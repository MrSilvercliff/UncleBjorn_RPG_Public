using _Project.Scripts.GameScene.Abilities;
using _Project.Scripts.GameScene.Creatures.Basis.Components;
using _Project.Scripts.GameScene.Services.Input;
using _Project.Scripts.Project.Animations;
using _Project.Scripts.Project.Enums;
using _Project.Scripts.Project.Services.Balance.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Project.Scripts.GameScene.Creatures.Player.States
{
    public interface IPlayerStateMove : IPlayerStateControllerBase
    { 
    }

    public class PlayerStateMove : PlayerStateControllerBase<PlayerStateMove>, IPlayerStateMove
    {
        public override StateMachineStateType StateType => StateMachineStateType.CreatureState_Move;

        [Inject] private IInputController _inputController;
        [Inject] private ICreatureHelper _creatureHelper;

        private IAbilityBasicMove _abilityMove;

        private IMonoBehaviourAnimatorController _animatorController;

        private CharacterController _characterController;
        private Vector3 _groundCheckOffest;
        private float _groundCheckRadius;
        private LayerMask _groundLayers;

        private Transform _creatureTransform;
        private Transform _cameraTransform;

        private float _gravity;
        private float _verticalVelocity;
        private float _rotationLerpT;
        private bool _isGrounded;

        public PlayerStateMove(IPlayerController creatureController) : base(creatureController)
        {
            _verticalVelocity = 0;
            _rotationLerpT = 0f;
            _isGrounded = true;
        }
        
        protected override void OnInit()
        {
            _abilityMove = _creatureModel.GetAbility<IAbilityBasicMove>(AbilityType.BASIC_MOVE);

            var componentAnimatorContorller = _creatureComponentContainer.GetComponent<ICreatureComponentAnimator>(CreatureComponentType.AnimatorController);
            _animatorController = componentAnimatorContorller.AnimatorController;

            var componentCharacterController = _creatureComponentContainer.GetComponent<ICreatureComponentCharacterController>(CreatureComponentType.CharacterController);
            _characterController = componentCharacterController.CharacterController;
            _groundCheckOffest = componentCharacterController.GroundCheckOffset;
            _groundCheckRadius = componentCharacterController.GroundCheckRadius;
            _groundLayers = componentCharacterController.GroundLayers;

            var componentCameraLook = _creatureComponentContainer.GetComponent<ICreatureComponentCameraLook>(CreatureComponentType.CameraLook);
            _creatureTransform = componentCameraLook.CreatureTransform;
            _cameraTransform = componentCameraLook.CameraTransform;

            _gravity = Physics.gravity.y * componentCharacterController.GravityMultiplier;
        }

        protected override void OnEnter()
        {
            _rotationLerpT = 0f;

            _animatorController.CurrentH = _animatorController.GetFloat(AnimatorParameter.H);
            _animatorController.CurrentV = _animatorController.GetFloat(AnimatorParameter.V);
        }


        public override void OnFixedUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;

            DoGroundCheck();
            DoGravity(deltaTime);
            DoMove(deltaTime);
            DoCameraAlignment(deltaTime);
            TryPlayAnimation();

        }

        public override void OnUpdate()
        {
        }

        public override void OnLateUpdate()
        {
        }

        protected override void OnExit()
        {
        }

        public override void OnAnimationFinished(int finishedState)
        {
        }

        private void DoGroundCheck()
        {
            var creaturePosition = _creatureController.Transform.position;
            _isGrounded = _creatureHelper.IsGrounded(creaturePosition, _groundCheckOffest, _groundCheckRadius, _groundLayers);
        }

        private void DoGravity(float deltaTime)
        {
            var jumpStateActive = _creatureStateMachine.IsStateActive(StateMachineStateType.CreatureState_Jump);
            var freeFallStateActive = _creatureStateMachine.IsStateActive(StateMachineStateType.CreatureState_FreeFall);

            if (jumpStateActive || freeFallStateActive)
            {
                _verticalVelocity = 0f;
                return;
            }

            if (_isGrounded)
            {
                _verticalVelocity = -_abilityMove.MoveSpeedForward;
                _verticalVelocity += _gravity * deltaTime;
                return;
            }

            _verticalVelocity = 0f;
            _creatureStateMachine.EnterState(StateMachineStateType.CreatureState_FreeFall);
        }

        private void DoMove(float deltaTime)
        {
            var horizontalVelocity = CalculateHorizontalVelocity(_inputController.MoveInput) * deltaTime;
            var verticalVelocity = new Vector3(0f, _verticalVelocity, 0f);

            var sumVelocity = horizontalVelocity + verticalVelocity;
            _characterController.Move(sumVelocity);
        }

        private Vector3 CalculateHorizontalVelocity(Vector3 moveInput)
        {
            var forward = CalculateForwardVector(moveInput.z);
            var strafe = CalculateStrafeVector(moveInput.x);

            var result = forward + strafe;
            return result;
        }

        private Vector3 CalculateForwardVector(float moveInputZ)
        {
            var result = Vector3.zero;
            var forward = _creatureTransform.forward;

            if (moveInputZ > 0)
                result = forward * moveInputZ * _abilityMove.MoveSpeedForward;
            else if (moveInputZ < 0)
                result = forward * moveInputZ * _abilityMove.MoveSpeedBackward;

            return result;
        }

        private Vector3 CalculateStrafeVector(float moveInputX)
        {
            var right = _creatureTransform.right;

            var result = right * moveInputX * _abilityMove.MoveSpeedStrafe;

            return result;
        }

        private void DoCameraAlignment(float deltaTime)
        {
            var freeLookStateIsActive = _creatureStateMachine.IsStateActive(StateMachineStateType.CreatureState_FreeLook);
            var rotateLookIsActive = _creatureStateMachine.IsStateActive(StateMachineStateType.CreatureState_RotateLook);

            if (freeLookStateIsActive || rotateLookIsActive)
            {
                _rotationLerpT = 0f;
                return;
            }

            _rotationLerpT += deltaTime;

            var rotation = Quaternion.Lerp(_cameraTransform.rotation, _creatureTransform.rotation, _rotationLerpT);
            var eulerAngles = rotation.eulerAngles;
            eulerAngles.z = 0f;
            _cameraTransform.eulerAngles = eulerAngles;
        }

        private void TryPlayAnimation()
        {
            var jumpStateActive = _creatureStateMachine.IsStateActive(StateMachineStateType.CreatureState_Jump);

            if (jumpStateActive)
                return;

            var moveInput = _inputController.MoveInput;

            float tempVelocityH = _animatorController.VelocityH;
            float tempVelocityV = _animatorController.VelocityV;

            _animatorController.CurrentH = Mathf.SmoothDamp(_animatorController.CurrentH, moveInput.x, ref tempVelocityH, .1f);
            _animatorController.CurrentV = Mathf.SmoothDamp(_animatorController.CurrentV, moveInput.z, ref tempVelocityV, .1f);

            _animatorController.VelocityH = tempVelocityH;
            _animatorController.VelocityV = tempVelocityV;

            _animatorController.SetFloat(AnimatorParameter.H, _animatorController.CurrentH);
            _animatorController.SetFloat(AnimatorParameter.V, _animatorController.CurrentV);
        }
    }
}