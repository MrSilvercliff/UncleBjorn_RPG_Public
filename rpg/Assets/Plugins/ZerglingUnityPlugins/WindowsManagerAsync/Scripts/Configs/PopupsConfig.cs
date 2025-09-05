using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Configs;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Popups;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Configs
{
    public interface IPopupsConfig : IConfigBase, IWindowsConfig<PopupWindow>
    {
    }
}