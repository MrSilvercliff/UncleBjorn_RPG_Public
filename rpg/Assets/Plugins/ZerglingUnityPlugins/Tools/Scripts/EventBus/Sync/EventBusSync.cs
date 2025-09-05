using System;
using System.Collections.Generic;
using ZerglingUnityPlugins.Tools.Scripts.Singleton;

namespace ZerglingUnityPlugins.Tools.Scripts.EventBus.Sync
{
    public interface IEventBusSync
    {
        void Subscribe<T>(Action<T> callback) where T : struct, IEvent;
        void UnSubscribe<T>(Action<T> callback) where T : struct, IEvent;
        void Fire<T>(T evnt) where T : struct, IEvent;
    }

    public class EventBusSync : Singleton<EventBusSync>, IEventBusSync
    {
        private Dictionary<Type, EventHandlerBase> _handlers = new Dictionary<Type, EventHandlerBase>();

        public void Subscribe<T>(Action<T> callback) where T : struct, IEvent
        {
            var eventType = typeof(T);

            if (!_handlers.ContainsKey(eventType))
            {
                var handler = new EventHandlerSync<T>();
                _handlers[eventType] = handler;
            }
            
            (_handlers[eventType] as EventHandlerSync<T>).Subscribe(callback);
        }

        public void UnSubscribe<T>(Action<T> callback) where T :  struct, IEvent
        {
            var eventType = typeof(T);

            if (!_handlers.ContainsKey(eventType))
                return;
            
            (_handlers[eventType] as EventHandlerSync<T>).UnSubscribe(callback);
        }

        public void Fire<T>(T evnt) where T : struct, IEvent
        {
            var eventType = evnt.GetType();
            
            if (!_handlers.ContainsKey(eventType))
                return;
            
            (_handlers[eventType] as EventHandlerSync<T>).Fire(evnt);
        }
    }
}


