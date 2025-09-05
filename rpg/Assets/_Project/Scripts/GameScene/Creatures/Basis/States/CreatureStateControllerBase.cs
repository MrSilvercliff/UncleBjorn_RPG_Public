using _Project.Scripts.Project.Enums;
using _Project.Scripts.Project.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Mono;

namespace _Project.Scripts.GameScene.Creatures.Basis.States
{
    public interface ICreatureStateControllerBase : IStateMachineStateController
    {
        void OnAnimationFinished(int finishedState);
    }

    public abstract class CreatureStateControllerBase<TCreatureController> : StateMachineStateController, ICreatureStateControllerBase
        where TCreatureController : ICreatureController
    {
        protected TCreatureController _creatureController;
        protected ICreatureModel _creatureModel;
        protected ICreatureStateMachine _creatureStateMachine;
        protected ICreatureComponentContainer _creatureComponentContainer;

        public CreatureStateControllerBase(TCreatureController creatureController)
        { 
            _creatureController = creatureController;
            _creatureStateMachine = creatureController.CreatureStateMachine;
            _creatureComponentContainer = creatureController.CreatureComponentContainer;
        }

        public override void Init()
        {
            _creatureModel = _creatureController.CreatureModel;
            OnInit();
        }

        protected abstract void OnInit();

        public override void Enter()
        {
            OnEnter();
        }

        protected abstract void OnEnter();

        public override void Exit()
        {
            OnExit();
        }

        protected abstract void OnExit();

        public abstract void OnAnimationFinished(int finishedState);
    }
}