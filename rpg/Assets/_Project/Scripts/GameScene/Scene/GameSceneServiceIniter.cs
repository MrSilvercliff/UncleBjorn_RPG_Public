using _Project.Scripts.GameScene.GameLoop;
using _Project.Scripts.GameScene.Services.Creatures;
using _Project.Scripts.GameScene.Services.Input;
using _Project.Scripts.GameScene.Services.SpawnPoints;
using _Project.Scripts.Project.Scenes;
using _Project.Scripts.Project.Services.Audio;
using _Project.Scripts.Project.Services.ProjectSettings;
using System.Threading.Tasks;
using _Project.Scripts.GameScene.Services.GameLevels;
using Plugins.ZerglingUnityPlugins.Localization_JSON_Object.Scripts;
using Zenject;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Panels;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Popups;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Views;

namespace _Project.Scripts.GameScene.Scene
{
    public interface IGameSceneServiceIniter : ISceneServiceIniter
    { 
    }

    public class GameSceneServiceIniter : SceneServiceIniter, IGameSceneServiceIniter
    {
        #region First

        // windows
        [Inject] private IViewController _viewController;
        [Inject] private IPopupController _popupController;
        [Inject] private IPanelSettingsRepository _panelSettingsRepository;
        [Inject] private IPanelController _panelController;

        // audio
        [Inject] private IAudioPlayController _audioPlayController;
        [Inject] private IAudioSettingsController _audioSettingsController;
        [Inject] private IAudioMixingController _audioMixingController;

        #endregion First

        #region Second

        // input
        [Inject] private IInputController _inputController;
        [Inject] private IInputHandler _inputHandler;

        // creatures
        [Inject] private ICreatureControllerRepository _creatureControllerRepository;

        // spawn points
        [Inject] private ISpawnPointRepository _spawnPointRepository;
        [Inject] private ICreatureSpawnPointRepository _creatureSpawnPointRepository;
        
        // game levels
        [Inject] private IGameLevelService _gameLevelService;

        // game loop
        [Inject] private IGameLoopController _gameLoopController;

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
            AddService_Flushable(_viewController);
            AddService_Flushable(_popupController);
            AddService_Flushable(_panelSettingsRepository);
            AddService_Flushable(_panelController);

            AddService_Flushable(_audioPlayController);
            AddService_Flushable(_audioSettingsController);
            AddService_Flushable(_audioMixingController);

            var result = await InitServices();
            return result;
        }

        private async Task<bool> InitSecond()
        {
            AddService_Flushable(_inputController);
            AddService_Flushable(_inputHandler);

            AddService_Flushable(_creatureControllerRepository);
            
            AddService_Flushable(_spawnPointRepository);
            AddService_Flushable(_creatureSpawnPointRepository);

            AddService_Flushable(_gameLevelService);
            AddService_Flushable(_gameLoopController);

            var result = await InitServices();
            return result;
        }
    }
}