using System.Collections.Generic;
using UnityEngine;

namespace Plugins.ZerglingUnityPlugins.Tools.Scripts.ObjectPool.Pools
{
    public abstract class Pool<TKey, TObject> where TObject : class
    {
        private readonly Dictionary<TKey, TObject> _sourceObjectsDict = new Dictionary<TKey, TObject>();
        private readonly Dictionary<TKey, Stack<TObject>> _objectsStacks = new Dictionary<TKey, Stack<TObject>>();

        protected abstract TObject Instantiate(TObject obj);

        #region Public methods
        public void RegisterSourceObject(TObject obj)
        {
            if (obj == null)
            {
                Debug.LogError($"[{GetType().Name}] Trying to register NULL instead {typeof(TObject).Name}!");
                return;
            }

            _sourceObjectsDict[GetKey(obj)] = obj;
        }

        public void RegisterSourceObject(TKey key, TObject obj)
        {
            if (obj == null)
            {
                Debug.LogError($"[{GetType().Name}] Trying to register NULL instead {typeof(TObject).Name}! Key: {key}");
                return;
            }

            _sourceObjectsDict[key] = obj;
        }

        public TObject Get(TKey key)
        {
            TObject res = null;

            if (!_objectsStacks.TryGetValue(key, out Stack<TObject> pool))
            {
                pool = new Stack<TObject>();
                _objectsStacks[key] = pool;
            }

            if (pool.Count == 0)
            {
                if (!_sourceObjectsDict.TryGetValue(key, out TObject obj))
                {
                    Debug.LogError($"[{GetType().Name}] No source object at key '{key.ToString()}'");
                    return null;
                }

                res = Instantiate(obj);
            }
            else
                res = pool.Pop();

            return res;
        }

        public abstract void Push(TObject obj);
        #endregion

        #region Protected methods
        protected void Push(TKey key, TObject obj)
        {
            if (!_objectsStacks.TryGetValue(key, out Stack<TObject> stack))
            {
                stack = new Stack<TObject>();
                _objectsStacks[key] = stack;
            }

            stack.Push(obj);
        }

        protected abstract TKey GetKey(TObject obj);
        #endregion
    }
}