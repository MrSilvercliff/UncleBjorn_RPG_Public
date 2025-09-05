using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Plugins.ZerglingUnityPlugins.Tools.Scripts.ObjectPool.Pools
{
    public interface IZFactoryMonoPool<T> where T : Component
    {
        void SetSource(T source);
        T Get();
        void Push(T obj);
        void WarningUp(int count);
        void Flush();
    }

    public class ZFactoryMonoPool<T> : IZFactoryMonoPool<T> where T : Component
    {
        private T _sourceObject;
        private readonly Stack<T> _stack = new Stack<T>();
        private Transform _parent;
        private readonly IFactory<T, T> _factory;

        public ZFactoryMonoPool(T source, IFactory<T, T> factory, Transform parent = null)
        {
            _sourceObject = source;
            _parent = parent == null ? _sourceObject.transform.parent : parent;
            _factory = factory;
        }

        public void SetSource(T source)
        {
            _sourceObject = source;
        }

        public T Get()
        {
            T obj = _stack.Count > 0 ? _stack.Pop() : Instantiate();
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
                Push(Instantiate());
        }

        private T Instantiate()
        {
            T obj = _factory.Create(_sourceObject);
            obj.transform.parent = _parent;
            return obj;
        }

        public void Flush()
        {
            while (_stack.Count > 0)
            { 
                var obj = _stack.Pop();
                GameObject.Destroy(obj.gameObject);
            }
        }
    }
}

