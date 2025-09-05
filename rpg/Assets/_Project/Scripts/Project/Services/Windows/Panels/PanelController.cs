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
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Panels;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Panels;
using ZerglingUnityPlugins.ZenjectExtentions.ContextProvider;

namespace _Project.Scripts.Project.Services.Windows.Panels
{
    public class PanelController : PanelControllerBase
    {
        [SerializeField] private Transform _panelsContainer;

        [Inject] private IZenjectContextProvider _contextProvider;
        [Inject] private ISceneLoadController _sceneLoadController;

        private Dictionary<Type, IPanelWindow> _projectPanels;
        private Dictionary<Type, IPanelWindow> _scenePanels;

        protected override async Task<bool> OnInit()
        {
            await InitProjectPanels();
            await InitScenePanels();
            return true;
        }

        private async Task InitProjectPanels()
        {
            if (_projectPanels != null)
            {
                var diContainer = _contextProvider.Container;

                foreach (var panelItem in _projectPanels)
                {
                    var panel = (PanelWindow)panelItem.Value;
                    diContainer.InjectGameObject(panel.gameObject);
                    await panel.Init();
                }

                return;
            }

            _projectPanels = new Dictionary<Type, IPanelWindow>();

            var getObject = new WindowConfigGetObject(SceneName.Project);
            var panelsList = _panelsConfig.GetWindowsList(getObject);
            await InitPanelsList(_projectPanels, panelsList);
        }

        private async Task InitScenePanels()
        {
            if (_scenePanels == null)
                _scenePanels = new Dictionary<Type, IPanelWindow>();

            var sceneName = _sceneLoadController.CurrentSceneName;
            var getObject = new WindowConfigGetObject(sceneName);
            var panelsList = _panelsConfig.GetWindowsList(getObject);
            await InitPanelsList(_scenePanels, panelsList);
        }

        private async Task InitPanelsList(Dictionary<Type, IPanelWindow> dictionary, IReadOnlyCollection<PanelWindow> list)
        {
            var diContainer = _contextProvider.Container;

            foreach (var panel in list)
            {
                var gameObject = diContainer.InstantiatePrefab(panel, _panelsContainer);
                var component = gameObject.GetComponent<IPanelWindow>();
                var panelType = component.GetType();
                await component.Init();

                dictionary[panelType] = component;
            }
        }

        protected override bool OnFlush()
        {
            foreach (var panelItem in _scenePanels)
            {
                var panel = (Window)panelItem.Value;
                panel.Flush();
                DestroyImmediate(panel);
            }

            _scenePanels.Clear();
            return true;
        }

        protected override IPanelWindow GetPanel(Type panelType)
        {
            IPanelWindow result = null;

            if (_projectPanels == null || _scenePanels == null)
                return null;

            var getResult = _projectPanels.TryGetValue(panelType, out result);
            if (getResult)
                return result;

            getResult = _scenePanels.TryGetValue(panelType, out result);
            if (getResult)
                return result;

            LogUtils.Error(this, $"there is no panel with type {panelType.Name}");
            return null;
        }

        protected override Task OnPanelOpened(IPanelWindow panel)
        {
            return Task.CompletedTask;
        }

        protected override Task OnPanelClosed(IPanelWindow panel)
        {
            return Task.CompletedTask;
        }
    }
}