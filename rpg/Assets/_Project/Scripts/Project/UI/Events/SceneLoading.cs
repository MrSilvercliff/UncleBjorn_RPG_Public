using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.EventBus;

namespace _Project.Scripts.Project.UI.Events
{
    public class SceneLoadingProgressEvent : IEvent
    {
        public float Progress { get; }

        public SceneLoadingProgressEvent(float progress)
        {
            Progress = progress;
        }
    }
}