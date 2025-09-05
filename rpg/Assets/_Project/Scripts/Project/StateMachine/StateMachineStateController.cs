using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Mono;

namespace _Project.Scripts.Project.StateMachine
{
    public interface IStateMachineStateController : IMonoFixedUpdatable, IMonoUpdatable, IMonoLateUpdatable
    {
        StateMachineStateType StateType { get; }
        void Init();
        void Enter();
        void Exit();
    }

    public abstract class StateMachineStateController : IStateMachineStateController
    {
        public abstract StateMachineStateType StateType { get; }

        public abstract void Init();

        public abstract void Enter();

        public abstract void OnFixedUpdate();

        public abstract void OnUpdate();

        public abstract void OnLateUpdate();

        public abstract void Exit();
    }
}