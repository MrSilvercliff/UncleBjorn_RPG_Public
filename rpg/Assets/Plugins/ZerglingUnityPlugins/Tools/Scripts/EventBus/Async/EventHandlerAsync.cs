using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace ZerglingUnityPlugins.Tools.Scripts.EventBus.Async
{
    public interface IEventHandlerAsync<T> : IEventHandlerBase
    {
        void Subscribe(Func<T, Task> callback);
        void UnSubscribe(Func<T, Task> callback);
        Task<bool> Fire(T evnt);
    }

    public class EventHandlerAsync<T> : EventHandlerBase, IEventHandlerAsync<T>
    {
        private HashSet<Func<T, Task>> _callbacks;
        private HashSet<Func<T, Task>> _callbacksToAdd;
        private HashSet<Func<T, Task>> _callbacksToRemove;

        private bool _isFiring;

        public EventHandlerAsync()
        { 
            _callbacks = new HashSet<Func<T, Task>>();
            _callbacksToAdd = new HashSet<Func<T, Task>>();
            _callbacksToRemove = new HashSet<Func<T, Task>>();

            _isFiring = false;
        }

        public void Subscribe(Func<T, Task> callback)
        {
            if (_isFiring)
            {
                _callbacksToAdd.Add(callback);
                return;
            }

            _callbacks.Add(callback);
        }

        public void UnSubscribe(Func<T, Task> callback)
        {
            if (_isFiring)
            {
                _callbacksToRemove.Remove(callback);
                return;
            }

            _callbacks.Remove(callback);
        }

        public async Task<bool> Fire(T evnt)
        {
            try
            {
                _isFiring = true;

                await Task.WhenAll(_callbacks.Select(callback => ((Func<T, Task>)callback)?.Invoke(evnt)));

                _isFiring = false;

                if (_callbacksToAdd.Count > 0)
                {
                    foreach (var callback in _callbacksToAdd)
                        _callbacks.Add(callback);
                }

                if (_callbacksToRemove.Count > 0)
                {
                    foreach (var callback in _callbacksToAdd)
                        _callbacks.Remove(callback);
                }
            }
            catch (Exception e) 
            {
                LogUtils.Error(this, e.Message);
                LogUtils.Error(this, e.StackTrace);
                return false;
            }

            return true;
        }
    }
}


