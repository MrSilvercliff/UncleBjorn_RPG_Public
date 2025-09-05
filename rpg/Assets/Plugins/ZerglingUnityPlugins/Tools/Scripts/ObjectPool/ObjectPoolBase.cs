using System.Collections.Generic;

namespace ZerglingUnityPlugins.Tools.Scripts.ObjectPool
{
    public interface IObjectPoolBase<T>
    {
        T Get();
        void Return(T obj);
    }

    public abstract class ObjectPoolBase<T> : IObjectPoolBase<T>
    {
        protected int _objectsCount;
        protected List<T> _freeObjects;

        public ObjectPoolBase()
        {
            _objectsCount = 0;
            _freeObjects = new List<T>();
        }

        public abstract T Get();

        public abstract void Return(T obj);

        protected abstract T Create();
    }
}

