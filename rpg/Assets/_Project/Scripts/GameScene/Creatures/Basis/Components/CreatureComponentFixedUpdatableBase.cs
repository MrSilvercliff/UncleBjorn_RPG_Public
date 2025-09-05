using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Mono;

namespace _Project.Scripts.GameScene.Creatures.Basis.Components
{
    public interface ICreatureComponentFixedUpdatableBase : ICreatureComponentBase, IMonoFixedUpdatable
    { 
    }

    public abstract class CreatureComponentFixedUpdatableBase : CreatureComponentBase, ICreatureComponentFixedUpdatableBase
    {
        public override void Init(ICreatureComponentContainer componentContainer, ICreatureController creatureController)
        {
            componentContainer.SubscribeFixedUpdatable(this);
            OnInit(componentContainer, creatureController);
        }

        protected abstract void OnInit(ICreatureComponentContainer componentContainer, ICreatureController creatureController);

        public abstract void OnFixedUpdate();
    }
}