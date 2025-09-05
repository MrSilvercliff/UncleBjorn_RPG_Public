using _Project.Scripts.Project.Configs.Windows;
using _Project.Scripts.Project.Enums;
using _Project.Scripts.Project.Services.SceneLoading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Log;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Views;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Views;
using ZerglingUnityPlugins.ZenjectExtentions.ContextProvider;

namespace _Project.Scripts.Project.Services.Windows.Views
{
    public class ViewController : ViewControllerBase
    {
        [SerializeField] private Transform _viewContainer;

        [Inject] private IZenjectContextProvider _contextProvider;
        [Inject] private ISceneLoadController _sceneLoadController;

        private Dictionary<Type, IViewWindow> _projectViews;
        private Dictionary<Type, IViewWindow> _sceneViews;

        protected async override Task<bool> OnInit()
        {
            await InitProjectViews();
            await InitSceneViews();
            return true;
        }

        private async Task InitProjectViews()
        {
            if (_projectViews != null)
            {
                var diContainer = _contextProvider.Container;

                foreach (var viewItem in _projectViews)
                {
                    var view = (ViewWindow)viewItem.Value;
                    diContainer.InjectGameObject(view.gameObject);
                    await view.Init();
                }

                return;
            }

            _projectViews = new Dictionary<Type, IViewWindow>();

            var getObject = new WindowConfigGetObject(SceneName.Project);
            var viewsList = _viewsConfig.GetWindowsList(getObject);
            await InitViewsList(_projectViews, viewsList);
        }

        private async Task InitSceneViews()
        {
            if (_sceneViews == null)
                _sceneViews = new Dictionary<Type, IViewWindow>();

            var sceneName = _sceneLoadController.CurrentSceneName;
            var getObject = new WindowConfigGetObject(sceneName);
            var viewsList = _viewsConfig.GetWindowsList(getObject);
            await InitViewsList(_sceneViews, viewsList);
        }

        private async Task InitViewsList(Dictionary<Type, IViewWindow> dictionary, IReadOnlyCollection<ViewWindow> list)
        {
            var diContainer = _contextProvider.Container;

            foreach (var view in list)
            {
                var viewGameObject = diContainer.InstantiatePrefab(view, _viewContainer);
                var viewComponent = viewGameObject.GetComponent<IViewWindow>();
                var viewType = viewComponent.GetType();
                await viewComponent.Init();

                dictionary[viewType] = viewComponent;
            }
        }

        protected override bool OnFlush()
        {
            foreach (var dictItem in _sceneViews)
            {
                var view = (Window)dictItem.Value;
                view.Flush();
                DestroyImmediate(view.gameObject);
            }

            _sceneViews.Clear();
            return true;
        }

        protected override IViewWindow GetView(Type viewType)
        {
            IViewWindow result = null;
            var getResult = _projectViews.TryGetValue(viewType, out result);
            if (getResult)
                return result;

            getResult = _sceneViews.TryGetValue(viewType, out result);
            if (getResult)
                return result;

            LogUtils.Error(this, $"there is no view with type {viewType.Name}");
            return null;
        }

        protected override Task OnViewOpened(IViewWindow view)
        {
            return Task.CompletedTask;
        }

        protected override Task OnViewClosed(IViewWindow view)
        {
            return Task.CompletedTask;
        }
    }
}