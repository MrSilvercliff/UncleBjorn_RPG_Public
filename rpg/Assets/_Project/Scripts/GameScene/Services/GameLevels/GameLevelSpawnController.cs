using System.Threading.Tasks;
using _Project.Scripts.GameScene.Configs;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.ZenjectExtentions.ContextProvider;

namespace _Project.Scripts.GameScene.Services.GameLevels
{
    public interface IGameLevelSpawnController
    {
        Task<bool> SpawnGameLevel();
    }

    public class GameLevelSpawnController : IGameLevelSpawnController
    {
        [Inject] private IGameLevelsConfig _gameLevelsConfig;
        [Inject] private IGameLevelService _gameLevelService;
        [Inject] private IZenjectContextProvider _zenjectContextProvider;
        
        public async Task<bool> SpawnGameLevel()
        {
            var gameLevelId = _gameLevelService.GameLevelIdToInstantiate;
            var gameLevelPrefab = _gameLevelsConfig.GetGameObjectById(gameLevelId);
            
            var diContainer = _zenjectContextProvider.Container;
            var gameLevelObject = diContainer.InstantiatePrefab(gameLevelPrefab, Vector3.zero, Quaternion.identity, null);
            diContainer.InjectGameObject(gameLevelObject);

            await Task.Delay(1000);

            var controller = gameLevelObject.GetComponent<GameLevelController>();
            controller.OnAwake();

            return true;
        }
    }
}
