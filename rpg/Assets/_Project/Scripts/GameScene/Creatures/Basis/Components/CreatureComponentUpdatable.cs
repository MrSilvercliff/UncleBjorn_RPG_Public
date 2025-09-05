using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Mono;

namespace _Project.Scripts.GameScene.Creatures.Basis.Components
{
    public interface ICreatureComponentUpdatable : ICreatureComponentBase, IMonoUpdatable
    { 
    }

    public abstract class CreatureComponentUpdatable : CreatureComponentBase, ICreatureComponentUpdatable
    {
        public override void Init(ICreatureComponentContainer componentContainer, ICreatureController creatureController)
        {
            componentContainer.SubscribeUpdatable(this);
            OnInit(componentContainer, creatureController);
        }

        protected abstract void OnInit(ICreatureComponentContainer componentContainer, ICreatureController creatureController);

        public abstract void OnUpdate();
    }
}