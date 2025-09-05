using _Project.Scripts.GameScene.Creatures.Basis;
using _Project.Scripts.GameScene.Creatures.Basis.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameScene.Creatures.Player.States
{
    public interface IPlayerStateControllerBase : ICreatureStateControllerBase
    { 
    }

    public abstract class PlayerStateControllerBase<TFactoryType> : CreatureStateControllerBase<IPlayerController>, IPlayerStateControllerBase
        where TFactoryType : IPlayerStateControllerBase
    {
        protected PlayerStateControllerBase(IPlayerController creatureController) : base(creatureController)
        {
        }

        public class Factory : PlaceholderFactory<IPlayerController, TFactoryType> { }
    }
}