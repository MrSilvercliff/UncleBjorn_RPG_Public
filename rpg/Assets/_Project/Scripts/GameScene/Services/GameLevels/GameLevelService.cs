using System.Threading.Tasks;
using _Project.Scripts.GameScene.Configs;
using _Project.Scripts.GameScene.Enums;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;

namespace _Project.Scripts.GameScene.Services.GameLevels
{
    public interface IGameLevelService : IProjectSerivce
    {
        GameLevelId GameLevelIdToInstantiate { get; }
        IGameLevelController CurrentGameLevelController { get; }

        void SetCurrentGameLevelController(IGameLevelController gameLevelController);
    }

    public class GameLevelService : IGameLevelService
    {
        public GameLevelId GameLevelIdToInstantiate { get; private set; }
        public IGameLevelController CurrentGameLevelController { get; private set; }

        [Inject] private IGameLevelsConfig _gameLevelsConfig;

        public GameLevelService()
        {
            GameLevelIdToInstantiate = GameLevelId.NONE;
        }

        public Task<bool> Init()
        {
            _gameLevelsConfig.Init();
            
            if (_gameLevelsConfig.LoadDevelopLevel)
                GameLevelIdToInstantiate = _gameLevelsConfig.DevelopLevelId;
            else
                GameLevelIdToInstantiate = _gameLevelsConfig.NewGameLevelId;
            
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            return true;
        }
        
        public void SetCurrentGameLevelController(IGameLevelController gameLevelController)
        {
            CurrentGameLevelController = gameLevelController;
        }
    }
}
