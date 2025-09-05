using System.Collections.Generic;

namespace ZerglingUnityPlugins.Tools.Scripts.Repositories
{
    public interface IRepositoryBase<T>
    {
        int Count { get; }

        void Add(T item);
        IReadOnlyCollection<T> GetAll();
    }
}


