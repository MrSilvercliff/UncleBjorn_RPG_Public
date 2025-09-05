using _Project.Scripts.GameScene.Creatures.Basis.States;
using _Project.Scripts.Project.Enums;
using _Project.Scripts.Project.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameScene.Creatures.Basis
{
    public interface ICreatureStateMachine : IStateMachineBase
    {
        void Setup(ICreatureController creatureController);
        List<StateMachineStateType> GetActiveStates();
        bool TryGetState(StateMachineStateType state, out ICreatureStateControllerBase stateController);
        bool TryGetActiveState(StateMachineStateType state, out ICreatureStateControllerBase stateController);
        void OnAnimationFinished(int finishedState);
    }
}