using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace _Project.Scripts.Project.StateMachine
{
    public interface IStateMachineOneActive : IStateMachineBase
    { 
    }

    public abstract class StateMachineOneActive<TStateControllerType, TFactoryType> : StateMachineBase<TStateControllerType, TFactoryType>, IStateMachineOneActive
        where TStateControllerType : class, IStateMachineStateController
        where TFactoryType : class, IStateMachineBase
    {
        protected TStateControllerType _activeStateController;

        protected StateMachineOneActive()
        {
            _activeStateController = null;
        }

        protected override void OnFlush()
        {
            _activeStateController = null;
        }

        public override void EnterState(StateMachineStateType state)
        {
            _activeStateController?.Exit();
            var stateToEnter = _allStates[state];
            _activeStateController = stateToEnter;
            _activeStateController.Enter();
        }

        public override void ExitState(StateMachineStateType state)
        {
            LogUtils.Error(this, $"USE ENTER STATE ONLY");
        }

        public override bool IsStateActive(StateMachineStateType state)
        {
            if (_activeStateController == null)
                return false;

            return _activeStateController.StateType == state;
        }

        public override void OnFixedUpdate()
        {
            _activeStateController?.OnFixedUpdate();
        }

        public override void OnUpdate()
        {
            _activeStateController?.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            _activeStateController?.OnLateUpdate();
        }
    }
}