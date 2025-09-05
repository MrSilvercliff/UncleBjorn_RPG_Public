using _Project.Scripts.GameScene.Creatures.Basis.Components;
using _Project.Scripts.GameScene.Services.CameraLook;
using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.GameScene.Services.InteractableObjects;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameScene.Creatures.Player.States
{
    public interface IPlayerStateFreeLook : IPlayerStateLook
    { 
    }

    public class PlayerStateFreeLook : PlayerStateControllerBase<PlayerStateFreeLook>, IPlayerStateFreeLook
    {
        public override StateMachineStateType StateType => StateMachineStateType.CreatureState_FreeLook;

        [Inject] private IInteractableObjectService _interactableObjectService;

        private ICreatureComponentCameraLook _componentCameraLook;

        private Transform _cameraTransform;

        public PlayerStateFreeLook(IPlayerController creatureController) : base(creatureController)
        {
        }
        
        protected override void OnInit()
        {
            _componentCameraLook = _creatureComponentContainer.GetComponent<ICreatureComponentCameraLook>(CreatureComponentType.CameraLook);
            _cameraTransform = _componentCameraLook.CameraTransform;
        }

        protected override void OnEnter()
        {
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
            _interactableObjectService.SetInteractBlock(true);
            
            var applyLookResult = CameraLookHelper.ApplyLookInput(_cameraTransform.eulerAngles, lookInput);
            _cameraTransform.eulerAngles = applyLookResult;
        }

        protected override void OnExit()
        {
            var rotateLookStateActive = _creatureStateMachine.IsStateActive(StateMachineStateType.CreatureState_RotateLook);

            if (rotateLookStateActive)
                return;
            
            _interactableObjectService.SetInteractBlock(false);
        }

        public override void OnAnimationFinished(int finishedState)
        {
        }
    }
}