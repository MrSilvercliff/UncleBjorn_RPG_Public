using Assets.Plugins.ZerglingUnityPlugins.Tools.Scripts.Mono;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Views;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Configs;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Popups;
using ZerglingUnityPlugins.ZenjectExtentions.ContextProvider;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Popups;
using ZerglingUnityPlugins.Tools.Scripts.EventBus.Async;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Panels;
using _Project.Scripts.Project.Configs.AnimationSettings;
using _Project.Scripts.Project.Configs.Audio;
using _Project.Scripts.Project.Configs.Windows;
using _Project.Scripts.Project.Configs;
using _Project.Scripts.Project.Handlers.Localization;
using _Project.Scripts.Project.Services.Windows.Views;
using _Project.Scripts.Project.Services.Windows.Popups;
using _Project.Scripts.Project.Services.Windows.Panels;
using _Project.Scripts.Project.Services.Timers;
using _Project.Scripts.Project.Services.Time;
using _Project.Scripts.Project.Services.SceneLoading;
using _Project.Scripts.Project.Services.ProjectSettings;
using _Project.Scripts.Project.Services.Balance;
using _Project.Scripts.Project.Services.Audio;
using _Project.Scripts.Project.ObjectPools;
using _Project.Scripts.Project.Handlers.Windows.Views;
using _Project.Scripts.Project.Handlers.Windows.Popups;
using _Project.Scripts.Project.Handlers.Windows.Panels;
using _Project.Scripts.Project.Handlers.SceneLoading;
using _Project.Scripts.Project.Services;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.Configs;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.JSONParse;
using Plugins.ZerglingUnityPlugins.Localization_JSON_Object.Scripts;
using Plugins.ZerglingUnityPlugins.Localization_Total_JSON.Scripts;
using UnityEngine.Serialization;

namespace _Project.Scripts.Project.Zenject
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private ProjectObjectPoolContainers _objectPoolContainers;

        [SerializeField] private MonoUpdater _monoUpdater;

        [SerializeField] private SceneLoadController _sceneLoadController;

        [Header("VIEWS")]
        [SerializeField] private ViewConfig _viewConfig;
        [SerializeField] private ViewController _viewController;

        [Header("POPUPS")]
        [SerializeField] private PopupConfig _popupConfig;
        [SerializeField] private PopupController _popupController;

        [Header("PANELS")]
        [SerializeField] private PanelConfig _panelConfig;
        [SerializeField] private PanelController _panelController;
        
        [Header("SERVICES CONFIGS")]
        [SerializeField] private ProjectBalanceConfig _projectBalanceConfig;
        [SerializeField] private ProjectSettingsConfig _projectSettingsConfig;
        [SerializeField] private AudioConfig _audioConfig;
        [SerializeField] private LocalizationConfig _localizationConfig;

        [Header("OTHER CONFIGS")]
        [SerializeField] private AnimationSettingsConfig _animationSettingsConfig;

        public override void InstallBindings()
        {
            BindBasisServices();

            BindProjectServices();
        }

        protected void BindBasisServices()
        {
            BindConfigs();

            BindObjectPools();

            BindEventBus();

            BindZenjectExtensions();

            BindMonoUpdater();

            BindSceneLoadingServices();

            BindViewServices();

            BindPopupServices();

            BindPanelServices();

            BindBalanceServices();

            BindTimeServices();

            BindTimerServices();

            BindLocalizastionServices();

            BindProjectSettingsServices();

            BindAudioServices();
        }

        #region BasisSerices

        private void BindConfigs()
        {
            Container.Bind<IAnimationSettingsConfig>().FromInstance(_animationSettingsConfig).AsSingle();
        }

        private void BindObjectPools()
        {
            BindAudioObjectPools();
        }

        private void BindAudioObjectPools()
        {
            var poolItem = _objectPoolContainers.AudioSourceController;

            var prefab = poolItem.Prefab;
            var container = poolItem.Container;

            Container.BindMemoryPool<AudioSourceController, AudioSourceControllerPool>()
                .FromComponentInNewPrefab(prefab)
                .UnderTransform(container);
        }

        private void BindEventBus()
        {
            Container.Bind<IEventBusAsync>().To<EventBusAsync>().AsSingle();
        }

        private void BindZenjectExtensions()
        {
            Container.Bind<IZenjectContextProvider>().To<ZenjectContextProvider>().AsSingle();
        }

        private void BindMonoUpdater()
        {
            Container.Bind<IMonoUpdater>().FromInstance(_monoUpdater).AsSingle();
        }

        private void BindSceneLoadingServices()
        {
            Container.Bind<ISceneControllerProvider>().To<SceneControllerProvider>().AsSingle();
            Container.Bind<ISceneLoadHandler>().To<SceneLoadHandler>().AsSingle();
            Container.Bind<ISceneLoadController>().FromInstance(_sceneLoadController).AsSingle();
        }

        private void BindViewServices()
        {
            Container.Bind<IViewsConfig>().FromInstance(_viewConfig).AsSingle();
            Container.Bind<IViewHandler>().To<ViewHandler>().AsSingle();
            Container.Bind<IViewController>().FromInstance(_viewController).AsSingle();
        }

        private void BindPopupServices()
        {
            Container.BindFactory<PopupWindow, PopupWindow, PopupWindow.Factory>().FromFactory<PopupFactory>();
            Container.Bind<IPopupsConfig>().FromInstance(_popupConfig).AsSingle();
            Container.Bind<IPopupHandler>().To<PopupHandler>().AsSingle();
            Container.Bind<IPopupController>().FromInstance(_popupController).AsSingle();
        }

        private void BindPanelServices()
        {
            Container.Bind<IPanelsConfig>().FromInstance(_panelConfig).AsSingle();
            Container.Bind<IPanelSettingsRepository>().To<PanelSettingsRepository>().AsSingle();
            Container.Bind<IPanelHandler>().To<PanelHandler>().AsSingle();
            Container.Bind<IPanelController>().FromInstance(_panelController).AsSingle();
        }

        private void BindBalanceServices()
        {
            Container.Bind<IBalanceConfig>().FromInstance(_projectBalanceConfig).AsSingle();
            Container.Bind<IJSONParseHelper>().To<JsonParseHelper>().AsSingle();
            Container.Bind<IBalanceJSONParser>().To<BalanceJSONParser>().AsSingle();
            Container.Bind<IProjectBalanceStorage>().To<ProjectBalanceStorage>().AsSingle();
        }

        private void BindTimeServices()
        {
            Container.Bind<ITimeProvider>().To<TimeProvider>().AsSingle();
        }

        private void BindTimerServices()
        {
            Container.Bind<ITimerIdProvider>().To<TimerIdProvider>().AsSingle();
            Container.Bind<ITimerController>().To<TimerController>().AsSingle();
        }

        private void BindLocalizastionServices()
        {
            Container.Bind<ILocalizationConfig>().FromInstance(_localizationConfig).AsSingle();
            Container.Bind<ILocalizationServiceHandler>().To<LocalizationServiceHandler>().AsSingle();
            Container.Bind<ILocalizationService>().To<LocalizationService>().AsSingle();
        }

        private void BindProjectSettingsServices()
        {
            Container.Bind<IProjectSettingsConfig>().FromInstance(_projectSettingsConfig).AsSingle();
            Container.Bind<IProjectSettingsController>().To<ProjectSettingsController>().AsSingle();
        }

        private void BindAudioServices()
        {
            Container.Bind<IAudioConfig>().FromInstance(_audioConfig).AsSingle();
            Container.Bind<IAudioPlayController>().To<AudioPlayController>().AsSingle();
            Container.Bind<IAudioSettingsController>().To<AudioSettingsController>().AsSingle();
            Container.Bind<IAudioMixingController>().To<AudioMixingController>().AsSingle();
        }

        #endregion BasisServices

        private void BindProjectServices()
        {
            BindOtherServices();
        }

        private void BindOtherServices()
        {
            Container.Bind<IMathHelpher>().To<MathHelper>().AsSingle();
        }
    }
}