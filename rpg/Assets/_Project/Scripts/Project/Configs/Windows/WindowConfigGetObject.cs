using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Configs;

namespace _Project.Scripts.Project.Configs.Windows
{
    public interface IWindowConfigGetObject : IWindowsConfigGetObjectBase
    {
        SceneName SceneName { get; }
    }

    public class WindowConfigGetObject : IWindowConfigGetObject
    {
        public SceneName SceneName => _sceneName;

        private SceneName _sceneName;

        public WindowConfigGetObject(SceneName sceneName)
        {
            _sceneName = sceneName;
        }
    }
}