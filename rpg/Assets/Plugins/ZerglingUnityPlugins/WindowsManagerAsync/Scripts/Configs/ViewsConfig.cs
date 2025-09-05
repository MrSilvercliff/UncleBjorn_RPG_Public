using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Configs;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Views;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Configs
{
    public interface IViewsConfig : IConfigBase, IWindowsConfig<ViewWindow>
    {
    }
}