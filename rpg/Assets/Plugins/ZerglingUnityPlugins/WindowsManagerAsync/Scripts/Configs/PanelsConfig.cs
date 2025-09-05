using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Configs;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Panels;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Configs
{
    public interface IPanelsConfig : IConfigBase, IWindowsConfig<PanelWindow>
    {
    }
}