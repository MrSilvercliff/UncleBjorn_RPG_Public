using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Configs;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Views;

namespace _Project.Scripts.Project.Configs.Windows
{
    [CreateAssetMenu(fileName = "ViewConfig", menuName = "Project/Configs/Project/Windows/ViewConfig")]
    public class ViewConfig : ScriptableObject, IViewsConfig
    {
        [SerializeField] private ViewWindow[] _projectViews;
        [SerializeField] private ViewWindow[] _startSceneViews;
        [SerializeField] private ViewWindow[] _mainSceneViews;
        [SerializeField] private ViewWindow[] _gameSceneViews;

        public void Init()
        {
        }

        public IReadOnlyCollection<ViewWindow> GetWindowsList(IWindowsConfigGetObjectBase getObject)
        {
            var getObj = (WindowConfigGetObject)getObject;

            switch (getObj.SceneName)
            {
                case SceneName.StartScene:
                    return _startSceneViews;

                case SceneName.MainScene:
                    return _mainSceneViews;

                case SceneName.GameScene:
                    return _gameSceneViews;
            }

            return _projectViews;
        }
    }
}