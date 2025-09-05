using _Project.Scripts.GameScene.Configs;
using _Project.Scripts.GameScene.Creatures.Basis;
using _Project.Scripts.GameScene.Scene;
using System.Collections;
using System.Collections.Generic;
using Plugins.ZerglingUnityPlugins.Tools.Scripts.ObjectPool.Pools;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameScene.Creatures.Basis.Prefab
{
    public interface ICreaturePrefabPool
    {
        ICreaturePrefab Spawn(string prefabId);
        void Despawn(CreaturePrefab prefab);
    }

    public class CreaturePrefabPool : ICreaturePrefabPool
    {
        [Inject] private IGameSceneObjectPoolContainers _objectPoolContainers;
        [Inject] private CreaturePrefab.Factory _creaturePrefabFactory;
        [Inject] private ICreaturePrefabConfig _prefabConfig;

        private Dictionary<string, IZFactoryMonoPool<CreaturePrefab>> _pools;

        public CreaturePrefabPool()
        {
            _pools = new();
        }

        public ICreaturePrefab Spawn(string id)
        {
            if (!_pools.ContainsKey(id))
                CreatePool(id);

            var pool = _pools[id];
            var result = pool.Get();
            return result;
        }

        public void Despawn(CreaturePrefab prefab)
        {
            var id = prefab.Id;
            var pool = _pools[id];
            pool.Push(prefab);

            var container = _objectPoolContainers.CreaturePrefab.Container;

            prefab.Transform.SetParent(container);
        }

        private void CreatePool(string id)
        {
            var container = _objectPoolContainers.CreaturePrefab.Container;

            var prefab = _prefabConfig.Get(id);

            var pool = new ZFactoryMonoPool<CreaturePrefab>(prefab, _creaturePrefabFactory, container);
            _pools[id] = pool;
        }
    }
}