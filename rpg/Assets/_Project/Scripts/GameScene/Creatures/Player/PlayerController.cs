using _Project.Scripts.GameScene.Creatures.Basis;
using _Project.Scripts.GameScene.Creatures.Player.States;
using _Project.Scripts.GameScene.Services.Input;
using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Project.Scripts.GameScene.Creatures.Player
{
    public interface IPlayerController : ICreatureController, IPlayerMovementInputListener
    { 
    }

    public class PlayerController : CreatureController, IPlayerController
    {
        public override CreatureType CreatureType => CreatureType.PLAYER;

        [Inject] private IInputController _inputController;

        #region BASIS

        protected override void OnCreateProcess()
        {
        }

        protected override void OnSpawnedProcess()
        {
            _inputController.SubscribeListener(this);
        }

        protected override void OnDespawnedProcess()
        {
            _inputController.UnSubscribeListener(this);
        }

        #endregion BASIS

        #region GAMEPLAY
        #endregion GAMEPLAY

        #region INPUT

        public void OnMoveInput(InputActionPhase phase, Vector3 moveInput)
        {
            switch (phase) 
            {
                case InputActionPhase.Started:
                    _creatureStateMachine.ExitState(StateMachineStateType.CreatureState_Idle);
                    _creatureStateMachine.EnterState(StateMachineStateType.CreatureState_Move);
                    break;

                case InputActionPhase.Canceled:
                    _creatureStateMachine.ExitState(StateMachineStateType.CreatureState_Move);
                    _creatureStateMachine.EnterState(StateMachineStateType.CreatureState_Idle);
                    break;
            }
        }

        public void OnFreeLookInput(InputActionPhase phase)
        {
            switch (phase) 
            {
                case InputActionPhase.Started:
                    _creatureStateMachine.EnterState(StateMachineStateType.CreatureState_FreeLook);
                    _creatureStateMachine.ExitState(StateMachineStateType.CreatureState_RotateLook);
                    break;

                case InputActionPhase.Canceled:
                    _creatureStateMachine.ExitState(StateMachineStateType.CreatureState_FreeLook);
                    break;
            }
        }

        public void OnRotateLookInput(InputActionPhase phase)
        {
            switch (phase)
            {
                case InputActionPhase.Started:
                    _creatureStateMachine.EnterState(StateMachineStateType.CreatureState_RotateLook);
                    _creatureStateMachine.ExitState(StateMachineStateType.CreatureState_FreeLook);
                    break;

                case InputActionPhase.Canceled:
                    _creatureStateMachine.ExitState(StateMachineStateType.CreatureState_RotateLook);
                    break;
            }
        }

        public void OnLookInput(InputActionPhase phase, Vector2 lookInput)
        {
            var freeLookStateTryGetValue = _creatureStateMachine.TryGetActiveState(StateMachineStateType.CreatureState_FreeLook, out var freeLookStateBase);
            var rotateLookStateTryGetValue = _creatureStateMachine.TryGetActiveState(StateMachineStateType.CreatureState_RotateLook, out var rotateLookStateBase);

            var freeLookState = (IPlayerStateLook)freeLookStateBase;
            var rotateLookState = (IPlayerStateLook)rotateLookStateBase;

            if (phase != InputActionPhase.Performed)
                return;

            freeLookState?.OnLook(lookInput);
            rotateLookState?.OnLook(lookInput);
        }

        public void OnJumpInput(InputActionPhase phase)
        {
            var jumpStateActive = _creatureStateMachine.IsStateActive(StateMachineStateType.CreatureState_Jump);

            if (jumpStateActive)
                return;

            switch (phase)
            {
                case InputActionPhase.Started:
                    _creatureStateMachine.EnterState(StateMachineStateType.CreatureState_Jump);
                    break;
            }
        }

        #endregion INPUT
    }
}