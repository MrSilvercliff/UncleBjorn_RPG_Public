using _Project.Scripts.GameScene.Creatures.Basis.Components;
using _Project.Scripts.Project.Animations;
using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameScene.Creatures.Player.States
{
    public interface IPlayerStateFreeFall : IPlayerStateControllerBase
    { 
    }

    public class PlayerStateFreeFall : PlayerStateControllerBase<PlayerStateFreeFall>, IPlayerStateFreeFall
    {
        public override StateMachineStateType StateType => StateMachineStateType.CreatureState_FreeFall;

        [Inject] private ICreatureHelper _creatureHelper;

        private IMonoBehaviourAnimatorController _animatorController;

        private CharacterController _characterController;
        private float _gravityMultiplier;
        private Vector3 _groundCheckOffest;
        private float _groundCheckRadius;
        private LayerMask _groundLayers;

        private float _verticalVelocity;
        private bool _isGrounded;

        public PlayerStateFreeFall(IPlayerController creatureController) : base(creatureController)
        {
            _verticalVelocity = 0;
            _isGrounded = true;
        }
        
        protected override void OnInit()
        {
            var componentAnimatorController = _creatureComponentContainer.GetComponent<ICreatureComponentAnimator>(CreatureComponentType.AnimatorController);
            _animatorController = componentAnimatorController.AnimatorController;

            var componentCharacterController = _creatureComponentContainer.GetComponent<ICreatureComponentCharacterController>(CreatureComponentType.CharacterController);
            _characterController = componentCharacterController.CharacterController;
            _gravityMultiplier = componentCharacterController.GravityMultiplier;
            _groundCheckOffest = componentCharacterController.GroundCheckOffset;
            _groundCheckRadius = componentCharacterController.GroundCheckRadius;
            _groundLayers = componentCharacterController.GroundLayers;
        }

        protected override void OnEnter()
        {
            _verticalVelocity = 0f;

            TryPlayAnimation();
        }

        public override void OnFixedUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;
            DoGroundCheck();

            if (_isGrounded)
            {
                _animatorController.CrossFade(AnimatorStateHash.JumpGrounding, .1f, 1);
                _creatureStateMachine.ExitState(StateType);
                return;
            }

            DoGravity(deltaTime);
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

        private void TryPlayAnimation()
        {
            _animatorController.CrossFade(AnimatorStateHash.JumpFall, .1f, 1);
        }

        private void DoGroundCheck()
        {
            var creaturePosition = _creatureController.Transform.position;
            _isGrounded = _creatureHelper.IsGrounded(creaturePosition, _groundCheckOffest, _groundCheckRadius, _groundLayers);
        }

        private void DoGravity(float deltaTime)
        {
            _verticalVelocity += Physics.gravity.y * _gravityMultiplier * deltaTime;
            var velocityVector = new Vector3(0f, _verticalVelocity, 0f) * deltaTime;
            _characterController.Move(velocityVector);
        }
    }
}