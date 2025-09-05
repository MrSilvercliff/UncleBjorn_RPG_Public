using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZerglingUnityPlugins.Tools.Scripts.ObjectPool
{
    public interface IMonoBehaviourObjectPool<T> : IObjectPoolBase<T>
    {
        void Setup(T prefab, Transform container, int initCount, int maxCount = int.MaxValue);
    }

    public class MonoBehaviourObjectPool<T>: ObjectPoolBase<T>, IMonoBehaviourObjectPool<T> where T : MonoBehaviour
    {
        private T _prefab;
        private Transform _container;
        private int _maxCount;

        private int _objectsCount;
        private List<T> _freeObjects;

        public void Setup(T prefab, Transform container, int initCount, int maxCount = int.MaxValue)
        {
            _freeObjects = new List<T>();
            _objectsCount = 0;

            _prefab = prefab;
            _container = container;
            _maxCount = maxCount;

            for (int i = 0; i < initCount; i++)
                Create();
        }

        public override T Get()
        {
            T result = null;

            if (_objectsCount == _maxCount)
                throw new Exception("Max objects count reached!");

            if (_freeObjects.Count == 0)
                result = Create();
            else
            {
                result = _freeObjects[0];
                _freeObjects.RemoveAt(0);
            }

            return result;
        }

        public override void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_container);
            _freeObjects.Add(obj);
        }

        protected override T Create()
        {
            var obj = GameObject.Instantiate(_prefab, _container);
            _freeObjects.Add(obj);
            obj.gameObject.SetActive(false);
            _objectsCount += 1;
            return obj;
        }
    }
}

