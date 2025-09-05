using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Configs;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Popups;

namespace _Project.Scripts.Project.Configs.Windows
{
    [CreateAssetMenu(fileName = "PopupConfig", menuName = "Project/Configs/Project/Windows/PopupConfig")]
    public class PopupConfig : ScriptableObject, IPopupsConfig
    {
        [SerializeField] private PopupWindow[] _projectPopups;
        [SerializeField] private PopupWindow[] _startScenePopups;
        [SerializeField] private PopupWindow[] _mainScenePopups;
        [SerializeField] private PopupWindow[] _gameScenePopups;

        public void Init()
        {
        }

        public IReadOnlyCollection<PopupWindow> GetWindowsList(IWindowsConfigGetObjectBase getObject)
        {
            var getObj = (WindowConfigGetObject)getObject;

            switch (getObj.SceneName)
            {
                case SceneName.StartScene:
                    return _startScenePopups;

                case SceneName.MainScene:
                    return _mainScenePopups;

                case SceneName.GameScene:
                    return _gameScenePopups;
            }

            return _projectPopups;
        }
    }
}