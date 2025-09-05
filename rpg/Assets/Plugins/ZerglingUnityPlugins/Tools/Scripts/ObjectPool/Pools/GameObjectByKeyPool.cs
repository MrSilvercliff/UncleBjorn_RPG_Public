using System.Collections.Generic;
using UnityEngine;

namespace Plugins.ZerglingUnityPlugins.Tools.Scripts.ObjectPool.Pools
{
    public class GameObjectByKeyPool<TKey>
    {
        private readonly Dictionary<TKey, SingleGameObjectPool> _subPools = new Dictionary<TKey, SingleGameObjectPool>();

        public void Push(TKey key, GameObject obj)
        {
            if (!_subPools.TryGetValue(key, out SingleGameObjectPool subPool))
            {
                Debug.LogError($"[{GetType().Name}] Try to push unregistered object '{obj.name}' to '{key}'!");
                return;
            }

            subPool.Push(obj);
        }

        public GameObject Get(TKey key, Transform parent = null, bool setActiveGameObject = true)
        {
            if (!_subPools.TryGetValue(key, out SingleGameObjectPool subPool))
            {
                Debug.LogError($"[{GetType().Name}] Object at '{key}' not found!");
                return null;
            }

            return subPool.Get(parent, setActiveGameObject);
        }

        public void SetSourceObject(TKey key, GameObject obj)
        {
            if (obj == null)
            {
                Debug.LogError($"[{GetType().Name}] Trying to register NULL instead GameObject! Key {key}");
                return;
            }

            if (!_subPools.TryGetValue(key, out SingleGameObjectPool subPool))
            {
                subPool = new SingleGameObjectPool();
                _subPools[key] = subPool;
            }

            subPool.SetSourceObject(obj);
        }

        public void Clear()
        {
            foreach (SingleGameObjectPool subPool in _subPools.Values)
                subPool.Clear();

            _subPools.Clear();
        }
    }
}

