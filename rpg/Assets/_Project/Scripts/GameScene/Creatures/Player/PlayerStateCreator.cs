using _Project.Scripts.GameScene.Creatures.Basis;
using _Project.Scripts.GameScene.Creatures.Player.States;
using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace _Project.Scripts.GameScene.Creatures.Player
{
    public interface IPlayerStateCreator : ICreatureStateCreator<IPlayerController, IPlayerStateControllerBase>
    {

    }

    public class PlayerStateCreator : IPlayerStateCreator
    {
        [Inject] private PlayerStateIdle.Factory _idleFactory;
        [Inject] private PlayerStateMove.Factory _moveFactory;
        [Inject] private PlayerStateFreeLook.Factory _freeLookFactory;
        [Inject] private PlayerStateRotateLook.Factory _rotateLookFactory;
        [Inject] private PlayerStateFreeFall.Factory _freeFallFactory;
        [Inject] private PlayerStateJump.Factory _jumpFactory;

        public IPlayerStateControllerBase Create(StateMachineStateType state, IPlayerController creatureController)
        {
            IPlayerStateControllerBase result = null;

            switch (state)
            {
                case StateMachineStateType.CreatureState_Idle:
                    result = _idleFactory.Create(creatureController);
                    break;

                case StateMachineStateType.CreatureState_Move:
                    result = _moveFactory.Create(creatureController);
                    break;

                case StateMachineStateType.CreatureState_FreeLook:
                    result = _freeLookFactory.Create(creatureController);
                    break;

                case StateMachineStateType.CreatureState_RotateLook:
                    result = _rotateLookFactory.Create(creatureController);
                    break;

                case StateMachineStateType.CreatureState_FreeFall:
                    result = _freeFallFactory.Create(creatureController);
                    break;

                case StateMachineStateType.CreatureState_Jump:
                    result = _jumpFactory.Create(creatureController);
                    break;

                default:
                    LogUtils.Error(this, $"Create not implemented for state [{state}]");
                    break;
            }

            return result;
        }
    }
}