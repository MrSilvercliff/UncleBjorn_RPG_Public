using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameScene.Creatures.Basis.Components
{
    public interface ICreatureComponentStateMachineDebug : ICreatureComponentBase
    { 
    }

    public class CreatureComponentStateMachineDebug : CreatureComponentBase, ICreatureComponentStateMachineDebug
    {
        [SerializeField] private List<StateMachineStateType> _activeStates;

        public override void Init(ICreatureComponentContainer componentContainer, ICreatureController creatureController)
        {
            var creatureStateMachine = creatureController.CreatureStateMachine;
            _activeStates = creatureController.CreatureStateMachine.GetActiveStates();
        }
    }
}