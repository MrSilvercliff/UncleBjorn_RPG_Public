using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Mono;

namespace _Project.Scripts.GameScene.Creatures.Basis.Components
{
    public interface ICreatureComponentLateUpdatable : ICreatureComponentBase, IMonoLateUpdatable
    { 
    }

    public abstract class CreatureComponentLateUpdatable : CreatureComponentBase, ICreatureComponentLateUpdatable
    {
        public override void Init(ICreatureComponentContainer componentContainer, ICreatureController creatureController)
        {
            componentContainer.SubscribeLateUpdatable(this);
            OnInit(componentContainer, creatureController);
        }

        protected abstract void OnInit(ICreatureComponentContainer componentContainer, ICreatureController creatureController);

        public abstract void OnLateUpdate();
    }
}