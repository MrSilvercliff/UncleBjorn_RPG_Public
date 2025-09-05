using ZerglingUnityPlugins.Tools.Scripts.EventBus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZerglingUnityPlugins.Tools.Scripts.EventBus.Async
{
    public interface IEventBusAsync
    {
        void Subscribe<T>(Func<T, Task> callback) where T : IEvent;
        void UnSubscribe<T>(Func<T, Task> callback) where T : IEvent;
        Task<bool> Fire<T>(T eve) where T : IEvent;
    }

    public class EventBusAsync : IEventBusAsync
    {
        private Dictionary<Type, EventHandlerBase> _handlers = new Dictionary<Type, EventHandlerBase>();

        public void Subscribe<T>(Func<T, Task> callback) where T : IEvent
        {
            var eventType = typeof(T);

            if (!_handlers.ContainsKey(eventType))
            {
                var handler = new EventHandlerAsync<T>();
                _handlers[eventType] = handler;
            }

            (_handlers[eventType] as EventHandlerAsync<T>).Subscribe(callback);
        }

        public void UnSubscribe<T>(Func<T, Task> callback) where T : IEvent
        {
            var eventType = typeof(T);

            if (!_handlers.ContainsKey(eventType))
                return;

            (_handlers[eventType] as EventHandlerAsync<T>).UnSubscribe(callback);
        }

        public async Task<bool> Fire<T>(T eve) where T : IEvent
        {
            var eventType = eve.GetType();

            if (!_handlers.ContainsKey(eventType))
                return false;

            var result = await (_handlers[eventType] as EventHandlerAsync<T>).Fire(eve);
            return result;
        }
    }
}

