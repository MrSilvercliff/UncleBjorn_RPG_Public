using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameScene.Creatures.Basis
{
    public abstract class CreatureControllerPool<TCreatureController> : MonoMemoryPool<TCreatureController>
        where TCreatureController : CreatureController
    {
        protected override void OnCreated(TCreatureController item)
        {
            base.OnCreated(item);
            item.OnCreated();
        }

        protected override void OnSpawned(TCreatureController item)
        {
            base.OnSpawned(item);
            item.OnSpawned();
        }

        protected override void OnDespawned(TCreatureController item)
        {
            base.OnDespawned(item);
            item.OnDespawned();
        }
    }
}