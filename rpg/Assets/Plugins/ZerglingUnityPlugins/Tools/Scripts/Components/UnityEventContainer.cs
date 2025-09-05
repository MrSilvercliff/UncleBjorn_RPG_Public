using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZerglingUnityPlugins.Tools.Scripts.Components
{
    [Serializable]
    public class UnityEventContainer
    {
        public UnityEvent Event => _event;

        [SerializeField] private UnityEvent _event;
    }
}