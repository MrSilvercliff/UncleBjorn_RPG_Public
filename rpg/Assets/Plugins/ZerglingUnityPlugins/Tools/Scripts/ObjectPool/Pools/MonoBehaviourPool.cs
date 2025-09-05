using System.Collections.Generic;
using UnityEngine;

namespace Plugins.ZerglingUnityPlugins.Tools.Scripts.ObjectPool.Pools
{
    public class MonoBehaviourPool<T> where T : Component
    {
        private T _sourceObject;
        private readonly Stack<T> _stack = new Stack<T>();
        private readonly Transform _parent;

        public MonoBehaviourPool(T source, Transform parent = null)
        {
            _sourceObject = source;

            _parent = parent == null ? _sourceObject.transform.parent : parent;
        }

        public void SetSource(T source)
        {
            _sourceObject = source;
        }

        public T Get()
        {
            T obj = _stack.Count > 0 ? _stack.Pop() : Object.Instantiate(_sourceObject, _parent);
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Push(T obj)
        {
            obj.gameObject.SetActive(false);
            _stack.Push(obj);
        }

        public void WarningUp(int count)
        {
            for (int i = 0; i < count; i++)
                Push(Object.Instantiate(_sourceObject, _parent));
        }
    }
}

