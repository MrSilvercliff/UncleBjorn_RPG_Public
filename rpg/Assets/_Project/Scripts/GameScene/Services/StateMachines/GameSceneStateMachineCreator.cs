using _Project.Scripts.GameScene.Creatures.Player;
using _Project.Scripts.Project.Enums;
using _Project.Scripts.Project.Services.StateMachines;
using _Project.Scripts.Project.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameScene.Services.StateMachines
{
    public class GameSceneStateMachineCreator : StateMachineCreator
    {
        [Inject] private PlayerStateMachine.Factory _playerFactory;

        protected override IStateMachineBase CreateStateMachineProcess(StateMachineType stateMachineType)
        {
            IStateMachineBase result = null;

            switch (stateMachineType)
            {
                case StateMachineType.CREATURE_PLAYER:
                    result = _playerFactory.Create();
                    break;
            }

            return result;
        }
    }
}