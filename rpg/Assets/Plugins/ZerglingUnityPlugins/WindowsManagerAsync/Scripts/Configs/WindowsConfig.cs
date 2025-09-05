using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Configs;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Popups;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Views;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Configs
{
    public interface IWindowsConfig<TWindow> where TWindow : IWindow
    {
        IReadOnlyCollection<TWindow> GetWindowsList(IWindowsConfigGetObjectBase getObject);
    }

    public interface IWindowsConfigGetObjectBase
    {
    }
}

