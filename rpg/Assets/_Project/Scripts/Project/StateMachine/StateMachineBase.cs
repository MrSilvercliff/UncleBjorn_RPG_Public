using System;
using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.Sync;
using ZerglingUnityPlugins.Tools.Scripts.Mono;
using IInitializable = ZerglingUnityPlugins.Tools.Scripts.Interfaces.Sync.IInitializable;

namespace _Project.Scripts.Project.StateMachine
{
    public interface IStateMachineBase : IInitializable, IFlushable, IMonoFixedUpdatable, IMonoUpdatable, IMonoLateUpdatable
    {
        void InitStates();
        void EnterState(StateMachineStateType state);
        void ExitState(StateMachineStateType state);
        bool IsStateActive(StateMachineStateType state);
    }

    public abstract class StateMachineBase<TStateControllerType, TFactoryType> : IStateMachineBase
        where TStateControllerType : class, IStateMachineStateController
        where TFactoryType : class, IStateMachineBase
    {
        protected Dictionary<StateMachineStateType, TStateControllerType> _allStates;

        protected StateMachineBase()
        {
            _allStates = new();
        }

        public bool Init()
        {
            CreateStateControllers();
            OnInit();
            return true;
        }

        protected abstract void CreateStateControllers();
        protected abstract void OnInit();

        public bool Flush()
        {
            OnFlush();
            return true;
        }

        protected abstract void OnFlush();

        public void InitStates()
        {
            foreach (var stateMachineStateController in _allStates.Values)
                stateMachineStateController.Init();
        }

        public abstract void EnterState(StateMachineStateType state);

        public abstract void ExitState(StateMachineStateType state);

        public abstract bool IsStateActive(StateMachineStateType state);

        public abstract void OnFixedUpdate();

        public abstract void OnUpdate();

        public abstract void OnLateUpdate();

        public class Factory : PlaceholderFactory<TFactoryType> { }
    }
}