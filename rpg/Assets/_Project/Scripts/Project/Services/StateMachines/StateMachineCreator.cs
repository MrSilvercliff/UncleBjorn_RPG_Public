using _Project.Scripts.Project.Enums;
using _Project.Scripts.Project.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace _Project.Scripts.Project.Services.StateMachines
{
    public interface IStateMachineCreator
    {
        IStateMachineBase CreateStateMachine(StateMachineType stateMachineType);
    }

    public abstract class StateMachineCreator : IStateMachineCreator
    {
        public IStateMachineBase CreateStateMachine(StateMachineType stateMachineType)
        {
            IStateMachineBase result = null;

            result = CreateStateMachineInternal(stateMachineType);

            if (result == null)
                result = CreateStateMachineProcess(stateMachineType);

            if (result == null)
                LogUtils.Error(this, $"Create does not implemented for type [{stateMachineType}]");

            return result;
        }

        private IStateMachineBase CreateStateMachineInternal(StateMachineType stateMachineType)
        {
            IStateMachineBase result = null;

            switch (stateMachineType)
            {
            }

            return result;
        }

        protected abstract IStateMachineBase CreateStateMachineProcess(StateMachineType stateMachineType);
    }
}