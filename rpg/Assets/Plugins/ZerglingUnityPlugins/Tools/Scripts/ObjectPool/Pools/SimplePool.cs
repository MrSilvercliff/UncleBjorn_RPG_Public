using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Plugins.ZerglingUnityPlugins.Tools.Scripts.ObjectPool.Pools
{
    public interface IPool<T>
    {
        void RegisterSourceObject(T sourceObject);
        T Get();
        void Push(T obj);
        void SetActionOnSpawn(Action<T> action);
        void SetActionOnDespawn(Action<T> action);
    }

    public interface IPoolable<T>
    {
        void Initialize(IPool<T> pool);
        void Despawn();
    }

    public class SimplePool<T> : IPool<T> where T : Component, IPoolable<T>
    {
        private readonly Stack<T> _stack = new Stack<T>();
        private T _sourceObject;

        private Action<T> _actionOnGet;
        private Action<T> _actionOnPush;

        public void RegisterSourceObject(T sourceObject)
        {
            _sourceObject = sourceObject;
        }

        public T Get()
        {
            T res = default;

            if (_stack.Count > 0)
            {
                res = _stack.Pop();
                _actionOnGet?.Invoke(res);
                return res;
            }

            if (_sourceObject != null)
            {
                res = Object.Instantiate(_sourceObject);
                res.Initialize(this);
                _actionOnGet?.Invoke(res);
                return res;
            }

            Debug.LogError($"[Pool<{nameof(T)}>] No source object!");
            return default;
        }

        public void Push(T obj)
        {
            _actionOnPush?.Invoke(obj);
            _stack.Push(obj);
        }

        public void SetActionOnSpawn(Action<T> action)
        {
            _actionOnGet = action;
        }

        public void SetActionOnDespawn(Action<T> action)
        {
            _actionOnPush = action;
        }
    }
}