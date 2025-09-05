using _Project.Scripts.GameScene.Creatures.Basis.Components;
using _Project.Scripts.Project.Animations;
using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace _Project.Scripts.GameScene.Creatures.Player.States
{
    public interface IPlayerStateIdle : IPlayerStateControllerBase
    { 
    }

    public class PlayerStateIdle : PlayerStateControllerBase<PlayerStateIdle>, IPlayerStateIdle
    {
        public override StateMachineStateType StateType => StateMachineStateType.CreatureState_Idle;

        [Inject] private ICreatureHelper _creatureHelper;

        private IMonoBehaviourAnimatorController _animatorController;

        private Vector3 _groundCheckOffest;
        private float _groundCheckRadius;
        private LayerMask _groundLayers;

        private bool _isGrounded;

        public PlayerStateIdle(IPlayerController creatureController) : base(creatureController)
        {
            _isGrounded = true;
        }
        
        protected override void OnInit()
        {
            var componentAnimatorController = _creatureComponentContainer.GetComponent<ICreatureComponentAnimator>(CreatureComponentType.AnimatorController);
            _animatorController = componentAnimatorController.AnimatorController;

            var componentCharacterController = _creatureComponentContainer.GetComponent<ICreatureComponentCharacterController>(CreatureComponentType.CharacterController);
            _groundCheckOffest = componentCharacterController.GroundCheckOffset;
            _groundCheckRadius = componentCharacterController.GroundCheckRadius;
            _groundLayers = componentCharacterController.GroundLayers;
        }

        protected override void OnEnter()
        {
            _animatorController.CurrentH = _animatorController.GetFloat(AnimatorParameter.H);
            _animatorController.CurrentV = _animatorController.GetFloat(AnimatorParameter.V);

            TryPlayAnimation();
        }

        public override void OnFixedUpdate()
        { 
            DoGroundCheck();

            if (_isGrounded)
                return;

            var jumpStateActive = _creatureStateMachine.IsStateActive(StateMachineStateType.CreatureState_Jump);

            if (jumpStateActive)
                return;

            _creatureStateMachine.EnterState(StateMachineStateType.CreatureState_FreeFall);
        }

        public override void OnUpdate()
        {
        }

        public override void OnLateUpdate()
        {
            TryPlayAnimation();
        }

        protected override void OnExit()
        {
        }

        public override void OnAnimationFinished(int finishedState)
        {
        }

        private void TryPlayAnimation()
        {
            var targetH = 0f;
            var targetV = 0f;

            float tempVelocityH = _animatorController.VelocityH;
            float tempVelocityV = _animatorController.VelocityV;

            _animatorController.CurrentH = Mathf.SmoothDamp(_animatorController.CurrentH, targetH, ref tempVelocityH, .1f);
            _animatorController.CurrentV = Mathf.SmoothDamp(_animatorController.CurrentV, targetV, ref tempVelocityV, .1f);

            _animatorController.VelocityH = tempVelocityH;
            _animatorController.VelocityV = tempVelocityV;

            _animatorController.SetFloat(AnimatorParameter.H, _animatorController.CurrentH);
            _animatorController.SetFloat(AnimatorParameter.V, _animatorController.CurrentV);
        }

        private void DoGroundCheck()
        {
            var creaturePosition = _creatureController.Transform.position;
            _isGrounded = _creatureHelper.IsGrounded(creaturePosition, _groundCheckOffest, _groundCheckRadius, _groundLayers);
        }
    }
}