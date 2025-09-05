using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZerglingUnityPlugins.Tools.Scripts.Interfaces.Sync
{
    public interface ILateStartable
    {
        bool OnLateStart();
    }
}