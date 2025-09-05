using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZerglingUnityPlugins.Tools.Scripts.Mono
{
    public interface IMonoLateUpdatable
    {
        void OnLateUpdate();
    }
}