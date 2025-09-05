using System.Collections.Generic;

namespace Plugins.ZerglingUnityPlugins.Tools.Scripts.ObjectPool.Pools
{
    public class SingleObjectPool<T> where T : class, new()
    {
        private readonly Stack<T> _stack = new Stack<T>();

        public T Get()
        {
            return _stack.Count > 0 ? _stack.Pop() : new T();
        }
        public void Push(T obj)
        {
            _stack.Push(obj);
        }
    }
}

