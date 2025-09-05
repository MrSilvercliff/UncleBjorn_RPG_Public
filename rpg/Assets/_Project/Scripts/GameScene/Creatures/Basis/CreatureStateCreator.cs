using _Project.Scripts.GameScene.Creatures.Basis.States;
using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameScene.Creatures.Basis
{
    public interface ICreatureStateCreator<TCreatureController, TCreatureStateBase>
        where TCreatureController : ICreatureController
        where TCreatureStateBase : ICreatureStateControllerBase
    {
        TCreatureStateBase Create(StateMachineStateType state, TCreatureController creatureController);
    }
}