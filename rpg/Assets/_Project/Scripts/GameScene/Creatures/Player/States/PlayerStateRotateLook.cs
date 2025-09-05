using _Project.Scripts.GameScene.Creatures.Basis.Components;
using _Project.Scripts.GameScene.Services.CameraLook;
using _Project.Scripts.Project.Animations;
using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.GameScene.InteractableObjects;
using _Project.Scripts.GameScene.Services.InteractableObjects;
using _Project.Scripts.Project.Services.Balance;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameScene.Creatures.Player.States
{
    public interface IPlayerStateRotateLook : IPlayerStateLook
    { 
    }

    public class PlayerStateRotateLook : PlayerStateControllerBase<PlayerStateRotateLook>, IPlayerStateRotateLook
    {
        public override StateMachineStateType StateType => StateMachineStateType.CreatureState_RotateLook;

        [Inject] private IInteractableObjectService _interactableObjectService;
        [Inject] private IProjectBalanceStorage _projectBalanceStorage;
        
        private IMonoBehaviourAnimatorController _animatorController;

        private Transform _cameraTransform;
        private Transform _creatureTransform;

        private Vector2 _lookInputSum;
        private float _interactBlockTreshold;

        public PlayerStateRotateLook(IPlayerController creatureController) : base(creatureController)
        {
        }
        
        protected override void OnInit()
        {
            var componentAnimatorController = _creatureComponentContainer.GetComponent<ICreatureComponentAnimator>(CreatureComponentType.AnimatorController);
            _animatorController = componentAnimatorController.AnimatorController;

            var componentCameraLook = _creatureComponentContainer.GetComponent<ICreatureComponentCameraLook>(CreatureComponentType.CameraLook);
            _cameraTransform = componentCameraLook.CameraTransform;
            _creatureTransform = componentCameraLook.CreatureTransform;
            
            var interactableObjectsBalance = _projectBalanceStorage.InteractableObjects;
            var interactBlockTreshold = interactableObjectsBalance.PlayerRotateLookInteractBlockTreshold;
            _interactBlockTreshold = interactBlockTreshold * interactBlockTreshold;
        }

        protected override void OnEnter()
        {
            _animatorController.CurrentH = _animatorController.GetFloat(AnimatorParameter.H);
            _animatorController.CurrentV = _animatorController.GetFloat(AnimatorParameter.V);

            _lookInputSum = Vector2.zero;
        }

        public override void OnFixedUpdate()
        {
        }

        public override void OnUpdate()
        {
        }

        public override void OnLateUpdate()
        {
        }

        public void OnLook(Vector2 lookInput)
        {
            _lookInputSum += lookInput;
            
            if (_lookInputSum.sqrMagnitude >= _interactBlockTreshold)
                _interactableObjectService.SetInteractBlock(true);
            
            var applyLookResult = CameraLookHelper.ApplyLookInput(_cameraTransform.eulerAngles, lookInput);
            _cameraTransform.eulerAngles = applyLookResult;

            var creatureEulerAngles = _creatureTransform.eulerAngles;
            var delta = applyLookResult.y - creatureEulerAngles.y;
            creatureEulerAngles.y = applyLookResult.y;
            _creatureTransform.eulerAngles = creatureEulerAngles;
            
            //TryPlayAnimation(delta);
        }

        protected override void OnExit()
        {
            var freeLookStateActive = _creatureStateMachine.IsStateActive(StateMachineStateType.CreatureState_FreeLook);

            if (freeLookStateActive)
                return;
            
            _interactableObjectService.SetInteractBlock(false);
        }

        public override void OnAnimationFinished(int finishedState)
        {
        }

        private void TryPlayAnimation(float rotationDelta)
        {
            var moveStateActive = _creatureStateMachine.IsStateActive(StateMachineStateType.CreatureState_Move);
            var jumpStateActive = _creatureStateMachine.IsStateActive(StateMachineStateType.CreatureState_Jump);

            if (moveStateActive || jumpStateActive)
                return;

            if (rotationDelta == 0)
                return;

            var stateHash = AnimatorStateHash.Idle;

            if (rotationDelta > 0)
                stateHash = AnimatorStateHash.TurnRight;
            else if (rotationDelta < 0)
                stateHash = AnimatorStateHash.TurnLeft;

            if (_animatorController.CurrentState == stateHash)
                return;

            _animatorController.CrossFade(stateHash, .1f, 2);
        }
    }
}