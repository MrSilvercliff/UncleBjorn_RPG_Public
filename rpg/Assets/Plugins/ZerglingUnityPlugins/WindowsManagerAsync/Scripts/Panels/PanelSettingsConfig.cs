using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Panels
{
    public interface IPanelSettingsConfig : IWindowSetup
    { 
        int CanvasSortOrder { get; set; }
    }

    public abstract class PanelSettingsConfig : ScriptableObject, IPanelSettingsConfig
    {
        public int CanvasSortOrder { get; set; }
    }
}