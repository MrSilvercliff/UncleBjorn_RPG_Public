using _Project.Scripts.Project.Scenes;
using _Project.Scripts.Project.Services.Audio;
using _Project.Scripts.Project.Services.Balance;
using _Project.Scripts.Project.Services.ProjectSettings;
using System.Threading.Tasks;
using Plugins.ZerglingUnityPlugins.Localization_JSON_Object.Scripts;
using Zenject;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Panels;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Popups;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Views;

namespace _Project.Scripts.StartScene.Scene
{
    public interface IStartSceneServiceIniter : ISceneServiceIniter
    { 
    }

    public class StartSceneServiceIniter : SceneServiceIniter, IStartSceneServiceIniter
    {
        #region First

        // balance
        [Inject] private IProjectBalanceStorage _balanceStorage;

        #endregion First

        #region Second

        // windows
        [Inject] private IViewController _viewController;
        [Inject] private IPopupController _popupController;
        [Inject] private IPanelSettingsRepository _panelSettingsRepository;
        [Inject] private IPanelController _panelController;

        // settings
        [Inject] private IProjectSettingsController _projectSettingsController;

        // localization
        [Inject] private ILocalizationService _localizationService;

        // audio
        [Inject] private IAudioPlayController _audioPlayController;
        [Inject] private IAudioSettingsController _audioSettingsController;
        [Inject] private IAudioMixingController _audioMixingController;

        #endregion Second

        protected override Task<bool> OnInit()
        {
            return Task.FromResult(true);
        }

        protected override async Task<bool> OnInitServices(int stage)
        {
            var result = true;

            switch (stage)
            {
                case 1:
                    result = await InitFirst();
                    break;

                case 2:
                    result = await InitSecond();
                    break;
            }

            return result;
        }

        private async Task<bool> InitFirst()
        {
            AddService_NonFlushable(_balanceStorage);

            var result = await InitServices();
            return result;
        }

        private async Task<bool> InitSecond()
        {
            AddService_Flushable(_viewController);
            AddService_Flushable(_popupController);
            AddService_Flushable(_panelSettingsRepository);
            AddService_Flushable(_panelController);

            AddService_NonFlushable(_projectSettingsController);

            AddService_NonFlushable(_localizationService);

            AddService_Flushable(_audioSettingsController);
            AddService_Flushable(_audioPlayController);
            AddService_Flushable(_audioMixingController);

            var result = await InitServices();
            return result;
        }
    }
}