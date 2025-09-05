using System.Collections.Generic;
using UnityEngine;

namespace Plugins.ZerglingUnityPlugins.Tools.Scripts.ObjectPool.Pools
{
    public abstract class ObjectByKeyPool<TKey, TObject> where TObject : Object
    {
        private readonly string name = $"ObjectByKeyPool<{nameof(TKey)}, {nameof(TObject)}>";

        private readonly Dictionary<TKey, TObject> _sourceObjectsDict = new Dictionary<TKey, TObject>();
        private readonly Dictionary<TKey, Stack<TObject>> _objectsStacks = new Dictionary<TKey, Stack<TObject>>();

        public void Clear()
        {
            foreach (Stack<TObject> stack in _objectsStacks.Values)
            {
                foreach (TObject o in stack)
                {
                    if (o != null)
                        DestroyObject(o);
                }
            }

            _objectsStacks.Clear();
            _sourceObjectsDict.Clear();
        }

        protected virtual void DestroyObject(TObject obj)
        {
            if (obj != null)
                Object.Destroy(obj);
        }

        public void SetSourceObject(TKey key, TObject obj)
        {
            if (obj == null)
            {
                Debug.LogError($"[{name}] Trying to register NULL instead {typeof(TObject).Name}! Key {key}");
                return;
            }

            _sourceObjectsDict[key] = obj;
        }

        public TObject Get(TKey key)
        {
            TObject res;

            if (!_objectsStacks.TryGetValue(key, out Stack<TObject> stack))
            {
                stack = new Stack<TObject>();
                _objectsStacks[key] = stack;
            }

            res = stack.Count == 0 ? Instantiate(key) : stack.Pop();

            if (res != null)
                return res;

            Debug.LogError($"[{name}] Required object is NULL!!! Key: {key}");
            stack.Clear();
            return Instantiate(key);
        }

        private TObject Instantiate(TKey key)
        {
            if (_sourceObjectsDict.TryGetValue(key, out TObject obj))
                return Object.Instantiate(obj);

            Debug.LogError($"[{name}] No source object at key '{key.ToString()}'");
            return null;
        }

        private bool TryInstantiate(TKey key, out TObject res)
        {
            if (_sourceObjectsDict.TryGetValue(key, out TObject obj))
            {
                res = Object.Instantiate(obj);
                return res != null;
            }

            res = null;
            return false;
        }

        public bool TryGet(TKey key, out TObject res)
        {
            if (!_objectsStacks.TryGetValue(key, out Stack<TObject> stack))
            {
                stack = new Stack<TObject>();
                _objectsStacks[key] = stack;
            }

            if (stack.Count == 0)
                return TryInstantiate(key, out res);

            res = stack.Pop();
            return true;
        }

        public void Push(TKey key, TObject obj)
        {
            if (!_objectsStacks.TryGetValue(key, out Stack<TObject> stack))
            {
                stack = new Stack<TObject>();
                _objectsStacks[key] = stack;
            }

            stack.Push(obj);
        }
    }
}

