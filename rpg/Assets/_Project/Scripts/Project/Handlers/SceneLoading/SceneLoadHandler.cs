using _Project.Scripts.Project.UI.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.EventBus.Async;

namespace _Project.Scripts.Project.Handlers.SceneLoading
{
    public interface ISceneLoadHandler
    {
        void OnSceneLoadingProgress(float progress);
    }

    public class SceneLoadHandler : ISceneLoadHandler
    {
        [Inject] private IEventBusAsync _eventBus;

        public void OnSceneLoadingProgress(float progress)
        {
            var eve = new SceneLoadingProgressEvent(progress);
            _eventBus.Fire(eve);
        }
    }
}