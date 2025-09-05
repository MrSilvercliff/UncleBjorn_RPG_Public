using System;
using System.Threading.Tasks;
using _Project.Scripts.GameScene.SpawnPoints;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.Async;
using IAwakable = ZerglingUnityPlugins.Tools.Scripts.Interfaces.Sync.IAwakable;

namespace _Project.Scripts.GameScene.Services.GameLevels
{
    public interface IGameLevelController : IAwakable, IStartable, ILateStartable
    {
    }

    public class GameLevelController : MonoBehaviour, IGameLevelController
    {
        [SerializeField] private SpawnPointContainer[] _spawnPointContainers;

        [Inject] private IGameLevelService _gameLevelService;
        
        public bool OnAwake()
        {
            _gameLevelService.SetCurrentGameLevelController(this);
            return true;
        }
        
        public async Task<bool> OnStart()
        {
            foreach (var spawnPointContainer in _spawnPointContainers)
                await spawnPointContainer.OnStart();
            
            return true;
        }

        public Task<bool> OnLateStart()
        {
            return Task.FromResult(true);
        }
    }
}
