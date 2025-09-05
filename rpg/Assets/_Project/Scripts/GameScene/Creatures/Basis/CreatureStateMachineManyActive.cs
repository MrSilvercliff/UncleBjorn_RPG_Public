using _Project.Scripts.GameScene.Creatures.Basis.States;
using _Project.Scripts.Project.Enums;
using _Project.Scripts.Project.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameScene.Creatures.Basis
{
    public abstract class CreatureStateMachineManyActive<TCreatureControllerType, TStateControllerType, TFactoryType> :
        StateMachineManyActive<TStateControllerType, TFactoryType>, ICreatureStateMachine
        where TCreatureControllerType : ICreatureController
        where TStateControllerType : class, ICreatureStateControllerBase
        where TFactoryType : class, IStateMachineBase
    {
        protected List<StateMachineStateType> _activeStates = new();
        protected TCreatureControllerType _creatureController;

        public void Setup(ICreatureController creatureController)
        {
            _creatureController = (TCreatureControllerType)creatureController;
        }

        public override void EnterState(StateMachineStateType state)
        {
            base.EnterState(state);

            if (_activeStates.Contains(state))
                return;

            _activeStates.Add(state);
        }

        public override void ExitState(StateMachineStateType state)
        {
            base.ExitState(state);

            if (!_activeStates.Contains(state))
                return;

            _activeStates.Remove(state);
        }

        public List<StateMachineStateType> GetActiveStates()
        {
            return _activeStates;
        }

        public bool TryGetState(StateMachineStateType state, out ICreatureStateControllerBase stateController)
        {
            var result = _allStates.TryGetValue(state, out var stateControllerBase);
            stateController = stateControllerBase;
            return result;
        }

        public bool TryGetActiveState(StateMachineStateType state, out ICreatureStateControllerBase stateController)
        {
            var result = _activeStatesDictionary.TryGetValue(state, out var stateControllerBase);
            stateController = stateControllerBase;
            return result;
        }

        public void OnAnimationFinished(int finishedState)
        {
            foreach (var state in _activeStatesHashSet)
                state.OnAnimationFinished(finishedState);
        }
    }
}