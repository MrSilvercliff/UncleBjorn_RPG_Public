using System.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Project.UI.Data
{
    public interface IUIData<T> : IUIData 
    {
        T GetData();
        void SetData(T item);
        
        void RefreshSync();
        Task<bool> RefreshAsync();
    }

    public abstract class UIData<T> : UIData, IUIData<T>
    {
        protected T _data;

        public T GetData()
        {
            return _data;
        }

        public void SetData(T item)
        {
            _data = item;

            if (_data == null)
                OnNullSet();
            else
                OnDataSet();
        }

        protected virtual void OnDataSet()
        {
        }

        protected virtual void OnNullSet()
        {
        }

        public virtual void RefreshSync()
        {
        }

        public virtual Task<bool> RefreshAsync()
        {
            return Task.FromResult(true);
        }
    }
}
