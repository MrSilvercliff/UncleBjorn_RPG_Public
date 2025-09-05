using _Project.Scripts.GameScene.Creatures.Basis;
using _Project.Scripts.GameScene.Creatures.Player.States;
using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameScene.Creatures.Player
{
    public class PlayerStateMachine : CreatureStateMachineManyActive<IPlayerController, IPlayerStateControllerBase, PlayerStateMachine>
    {
        [Inject] private IPlayerStateCreator _stateCreator;

        protected override void OnInit()
        {
        }

        protected override void CreateStateControllers()
        {
            var idleState = StateMachineStateType.CreatureState_Idle;
            _allStates[idleState] = _stateCreator.Create(idleState, _creatureController);

            var moveState = StateMachineStateType.CreatureState_Move;
            _allStates[moveState] = _stateCreator.Create(moveState, _creatureController);

            var freeLookState = StateMachineStateType.CreatureState_FreeLook;
            _allStates[freeLookState] = _stateCreator.Create(freeLookState, _creatureController);

            var rotateLookState = StateMachineStateType.CreatureState_RotateLook;
            _allStates[rotateLookState] = _stateCreator.Create(rotateLookState, _creatureController);

            var freefallState = StateMachineStateType.CreatureState_FreeFall;
            _allStates[freefallState] = _stateCreator.Create(freefallState, _creatureController);

            var jumpState = StateMachineStateType.CreatureState_Jump;
            _allStates[jumpState] = _stateCreator.Create(jumpState, _creatureController);
        }
    }
}