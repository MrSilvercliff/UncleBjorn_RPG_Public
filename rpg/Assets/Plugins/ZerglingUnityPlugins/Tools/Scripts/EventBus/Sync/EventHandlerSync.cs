using System;
using System.Collections.Generic;

namespace ZerglingUnityPlugins.Tools.Scripts.EventBus.Sync
{
    public interface IEventHandlerSync<T> : IEventHandlerBase
    {
        void Subscribe(Action<T> action);
        void UnSubscribe(Action<T> action);
        void Fire(T evnt);
    }

    public class EventHandlerSync<T>: EventHandlerBase, IEventHandlerSync<T>
    {
        private HashSet<Action<T>> _callbacks;
        private HashSet<Action<T>> _callbacksToAdd;
        private HashSet<Action<T>> _callbacksToRemove;

        private bool _isFiring;

        public EventHandlerSync()
        {
            _callbacks = new HashSet<Action<T>>();
            _callbacksToAdd = new HashSet<Action<T>>();
            _callbacksToRemove = new HashSet<Action<T>>();

            _isFiring = false;
        }

        public virtual void Subscribe(Action<T> callback)
        {
            if (_isFiring)
            {
                _callbacksToAdd.Add(callback);
                return;
            }

            _callbacks.Add(callback);
        }

        public virtual void UnSubscribe(Action<T> callback)
        {
            if (_isFiring) 
            {
                _callbacksToRemove.Remove(callback);
                return;
            }

            _callbacks.Remove(callback);
        }

        public virtual void Fire(T evnt)
        {
            _isFiring = true;

            foreach (var callback in _callbacks) 
                callback.Invoke(evnt);

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
    }
}


