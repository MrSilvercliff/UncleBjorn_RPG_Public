using ZerglingUnityPlugins.Tools.Scripts.ObjectPool;
using System;

namespace ZerglingUnityPlugins.Tools.Scripts.ObjectPool
{
    public interface IClassObjectPool<T> : IObjectPoolBase<T>
    {
        void Setup(int initCount, int maxCount);
    }

    public class ClassObjectPool<T> : ObjectPoolBase<T>, IClassObjectPool<T> where T : class, new()
    {
        private int _maxCount;

        public ClassObjectPool()
        {
            _maxCount = int.MaxValue;
        }

        public void Setup(int initCount, int maxCount)
        {
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
                var last = _freeObjects.Count - 1;
                result = _freeObjects[last];
                _freeObjects.RemoveAt(last);
            }

            return result;
        }

        public override void Return(T obj)
        {
            _freeObjects.Add(obj);
        }

        protected override T Create()
        {
            var obj = new T();
            _freeObjects.Add(obj);
            _objectsCount += 1;
            return obj;
        }
    }
}


