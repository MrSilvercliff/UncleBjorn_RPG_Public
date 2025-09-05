using _Project.Scripts.GameScene.Abilities;
using _Project.Scripts.GameScene.Creatures.Basis.Components;
using _Project.Scripts.Project.Animations;
using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameScene.Creatures.Player.States
{
    public interface IPlayerStateJump : IPlayerStateControllerBase
    { 
    }

    public class PlayerStateJump : PlayerStateControllerBase<PlayerStateJump>, IPlayerStateJump
    {
        public override StateMachineStateType StateType => StateMachineStateType.CreatureState_Jump;

        [Inject] private ICreatureHelper _creatureHelper;

        private IMonoBehaviourAnimatorController _animatorController;

        private IAbilityBasicMove _abilityMove;

        private CharacterController _characterController;
        private Vector3 _groundCheckOffest;
        private float _groundCheckRadius;
        private LayerMask _groundLayers;
        
        private float _verticalVelocityStart;
        private float _verticalVelocityCurrent;
        private float _gravity;
        private bool _isGrounded;

        public PlayerStateJump(IPlayerController creatureController) : base(creatureController)
        {
            _verticalVelocityCurrent = 0;
            _isGrounded = true;
        }
        
        protected override void OnInit()
        {
            _abilityMove = _creatureModel.GetAbility<IAbilityBasicMove>(AbilityType.BASIC_MOVE);

            var componentAnimatorController = _creatureComponentContainer.GetComponent<ICreatureComponentAnimator>(CreatureComponentType.AnimatorController);
            _animatorController = componentAnimatorController.AnimatorController;

            var componentCharacterController = _creatureComponentContainer.GetComponent<ICreatureComponentCharacterController>(CreatureComponentType.CharacterController);
            _characterController = componentCharacterController.CharacterController;
            _groundCheckOffest = componentCharacterController.GroundCheckOffset;
            _groundCheckRadius = componentCharacterController.GroundCheckRadius;
            _groundLayers = componentCharacterController.GroundLayers;

            _gravity = Physics.gravity.y * componentCharacterController.GravityMultiplier;
            _verticalVelocityStart = Mathf.Sqrt(-2f * _abilityMove.JumpHeight * _gravity);
        }

        protected override void OnEnter()
        {
            _verticalVelocityCurrent = _verticalVelocityStart;
            
            _animatorController.CrossFade(AnimatorStateHash.JumpStart, .1f, 1);

            _animatorController.CurrentH = _animatorController.GetFloat(AnimatorParameter.H);
            _animatorController.CurrentV = _animatorController.GetFloat(AnimatorParameter.V);
        }

        public override void OnFixedUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;
            DoMove(deltaTime);
            DoGravity(deltaTime);
            DoGroundCheck();

            if (!_isGrounded)
                return;

            if (_animatorController.CurrentState != AnimatorStateHash.JumpGrounding)
                _animatorController.CrossFade(AnimatorStateHash.JumpGrounding, .1f, 1);
            
            _creatureStateMachine.ExitState(StateMachineStateType.CreatureState_Jump);
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
            if (finishedState == AnimatorStateHash.JumpStart)
                _animatorController.Play(AnimatorStateHash.JumpFall, 1);
        }

        private void DoGroundCheck()
        {
            var creaturePosition = _creatureController.Transform.position;
            _isGrounded = _creatureHelper.IsGrounded(creaturePosition, _groundCheckOffest, _groundCheckRadius, _groundLayers);
        }

        private void DoMove(float deltaTime)
        {
            var moveVector = new Vector3(0, _verticalVelocityCurrent, 0) * deltaTime;
            _characterController.Move(moveVector);
        }

        private void DoGravity(float deltaTime)
        {
            _verticalVelocityCurrent += _gravity * deltaTime;
        }
    }
}