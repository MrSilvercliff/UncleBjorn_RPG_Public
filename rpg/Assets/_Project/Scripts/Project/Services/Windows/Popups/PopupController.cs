using _Project.Scripts.Project.Configs.Windows;
using _Project.Scripts.Project.Enums;
using _Project.Scripts.Project.Services.SceneLoading;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugins.ZerglingUnityPlugins.Tools.Scripts.ObjectPool.Pools;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Popups;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Popups;
using ZerglingUnityPlugins.ZenjectExtentions.ContextProvider;

namespace _Project.Scripts.Project.Services.Windows.Popups
{
    public class PopupController : PopupControllerBase
    {
        [SerializeField] private Transform _popupContainer;

        [Inject] private IZenjectContextProvider _contextProvider;
        [Inject] private ISceneLoadController _sceneLoadController;
        [Inject] private PopupWindow.Factory _popupFactory;

        private Dictionary<Type, PopupWindow> _projectPopups;
        private Dictionary<Type, PopupWindow> _scenePopups;

        private Dictionary<Type, IZFactoryMonoPool<PopupWindow>> _projectPools;
        private Dictionary<Type, IZFactoryMonoPool<PopupWindow>> _scenePools;

        protected async override Task<bool> OnInit()
        {
            await InitProjectPopups();
            await InitScenePopups();
            return true;
        }

        private async Task InitProjectPopups()
        {
            if (_projectPopups != null)
            {
                var diContainer = _contextProvider.Container;

                foreach (var popupItem in _projectPopups)
                {
                    var popup = popupItem.Value;
                    diContainer.InjectGameObject(popup.gameObject);
                    await popup.Init();
                }

                return;
            }

            _projectPopups = new Dictionary<Type, PopupWindow>();
            _projectPools = new Dictionary<Type, IZFactoryMonoPool<PopupWindow>>();

            var getObject = new WindowConfigGetObject(SceneName.Project);
            var popups = _popupsConfig.GetWindowsList(getObject);
            await InitPopupsList(_projectPopups, popups);
        }

        private async Task InitScenePopups()
        {
            if (_scenePopups == null)
            {
                _scenePopups = new Dictionary<Type, PopupWindow>();
                _scenePools = new Dictionary<Type, IZFactoryMonoPool<PopupWindow>>();
            }

            var sceneName = _sceneLoadController.CurrentSceneName;
            var getObject = new WindowConfigGetObject(sceneName);
            var popups = _popupsConfig.GetWindowsList(getObject);
            await InitPopupsList(_scenePopups, popups);
        }

        private Task InitPopupsList(Dictionary<Type, PopupWindow> dictionary, IReadOnlyCollection<PopupWindow> popups)
        {
            foreach (var popup in popups)
            {
                var popupType = popup.GetType();
                dictionary[popupType] = popup;
                popup.Init();
            }
            
            return Task.CompletedTask;
        }

        protected override bool OnFlush()
        {
            foreach (var dictItem in _scenePools)
            {
                var pool = dictItem.Value;
                pool.Flush();
            }

            _scenePools.Clear();
            _scenePopups.Clear();
            return true;
        }

        protected override IPopupWindow GetPopup(Type popupType)
        {
            var result = GetPopup(popupType, _projectPopups, _projectPools);

            if (result == null)
                result = GetPopup(popupType, _scenePopups, _scenePools);

            return result;
        }

        private IPopupWindow GetPopup(Type popupType, Dictionary<Type, PopupWindow> popupDictionary, Dictionary<Type, IZFactoryMonoPool<PopupWindow>> poolsDictionary)
        {
            if (!popupDictionary.ContainsKey(popupType))
                return null;

            if (!poolsDictionary.ContainsKey(popupType))
            {
                var popup = popupDictionary[popupType];
                var newPool = new ZFactoryMonoPool<PopupWindow>(popup, _popupFactory, _popupContainer);
                poolsDictionary[popupType] = newPool;
            }

            var pool = poolsDictionary[popupType];
            var result = pool.Get();

            ResetPopupTransform(result);

            result.Init();
            return result;
        }

        private void ResetPopupTransform(PopupWindow popup)
        {
            var popupTransform = (RectTransform)popup.transform;
            popupTransform.localScale = Vector3.one;
            popupTransform.anchoredPosition = Vector3.zero;
            Debug.Log(popupTransform.sizeDelta);
            popupTransform.sizeDelta = Vector3.zero;

            var position = popupTransform.localPosition;
            position.z = 0;

            popupTransform.localPosition = position;
        }
        
        protected override Task OnPopupOpened(IPopupWindow popup)
        {
            return Task.CompletedTask;
        }

        protected override async Task OnPopupClosed(IPopupWindow popup)
        {
            var popupType = popup.GetType();

            if (_projectPools.ContainsKey(popupType))
                await ReturnToPool((PopupWindow)popup, _projectPools);

            if (_scenePools.ContainsKey(popupType))
                await ReturnToPool((PopupWindow)popup, _scenePools);
        }

        private Task ReturnToPool(PopupWindow popupWindow, Dictionary<Type, IZFactoryMonoPool<PopupWindow>> poolsDictionary)
        {
            var popupType = popupWindow.GetType();
            var pool = poolsDictionary[popupType];
            pool.Push(popupWindow);
            return Task.CompletedTask;
        }
    }
}