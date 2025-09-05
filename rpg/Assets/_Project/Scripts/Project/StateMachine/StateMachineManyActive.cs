using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Project.StateMachine
{
    public interface IStateMachineManyActive : IStateMachineBase
    { 
    }

    public abstract class StateMachineManyActive<TStateControllerType, TFactoryType> : StateMachineBase<TStateControllerType, TFactoryType>, IStateMachineManyActive
        where TStateControllerType : class, IStateMachineStateController
        where TFactoryType : class, IStateMachineBase
    {
        protected Dictionary<StateMachineStateType, TStateControllerType> _activeStatesDictionary;
        protected HashSet<TStateControllerType> _activeStatesHashSet;

        private HashSet<TStateControllerType> _statesToEnter;
        private HashSet<TStateControllerType> _statesToExit;

        protected StateMachineManyActive()
        {
            _activeStatesDictionary = new();
            _activeStatesHashSet = new();
            _statesToEnter = new();
            _statesToExit = new();
        }

        protected override void OnFlush()
        {
            _activeStatesDictionary.Clear();
            _activeStatesHashSet.Clear();
            _statesToEnter.Clear();
            _statesToExit.Clear();
        }

        public override void EnterState(StateMachineStateType state)
        {
            var tryGetResult = _activeStatesDictionary.TryGetValue(state, out var stateController);

            if (stateController != null)
                return;

            var stateToEnter = _allStates[state];

            if (_statesToEnter.Contains(stateToEnter))
                return;

            _statesToEnter.Add(stateToEnter);
        }

        public override void ExitState(StateMachineStateType state)
        {
            var tryGetResult = _activeStatesDictionary.TryGetValue(state, out var stateController);

            if (stateController == null)
                return;

            var stateToExit = _allStates[state];

            if (_statesToExit.Contains(stateToExit))
                return;

            _statesToExit.Add(stateToExit);
        }

        public override bool IsStateActive(StateMachineStateType state)
        {
            var tryGetResult = _activeStatesDictionary.TryGetValue(state, out var stateController);

            if (tryGetResult == false) 
                return false;

            return stateController != null;
        }

        public override void OnFixedUpdate()
        {
            foreach (var state in _activeStatesHashSet)
                state.OnFixedUpdate();
        }

        public override void OnUpdate()
        {
            foreach (var state in _activeStatesHashSet)
                state.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            foreach (var state in _activeStatesHashSet)
                state.OnLateUpdate();

            CheckStatesToEnter();
            CheckStatesToExit();
        }

        protected void CheckStatesToEnter()
        {
            foreach (var state in _statesToEnter)
            {
                var stateType = state.StateType;
                _activeStatesDictionary[stateType] = state;
                _activeStatesHashSet.Add(state);
                state.Enter();
            }

            _statesToEnter.Clear();
        }

        protected void CheckStatesToExit()
        {
            foreach (var state in _statesToExit)
            { 
                var stateType = state.StateType;
                _activeStatesDictionary[stateType] = null;
                _activeStatesHashSet.Remove(state);
                state.Exit();
            }

            _statesToExit.Clear();
        }
    }
}