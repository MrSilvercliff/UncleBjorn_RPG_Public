using _Project.Scripts.GameScene.Configs;
using _Project.Scripts.GameScene.Services.Creatures;
using _Project.Scripts.GameScene.Services.SpawnPoints;
using Assets.Plugins.ZerglingUnityPlugins.Tools.Scripts.Mono;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Scripts.GameScene.Services.GameLevels;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.Async;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;
using ZerglingUnityPlugins.Tools.Scripts.Mono;

namespace _Project.Scripts.GameScene.GameLoop
{
    public interface IGameLoopController : IProjectSerivce, IStartable, ILateStartable, IMonoUpdatable, IMonoFixedUpdatable, IMonoLateUpdatable
    {
    }

    public class GameLoopController : IGameLoopController
    {
        [Inject] private ICreaturePrefabConfig _creaturePrefabConfig;

        [Inject] private IMonoUpdater _monoUpdater;

        [Inject] private ICreatureControllerRepository _creatureControllerRepository;
        [Inject] private ICreatureControllerUpdater _creatureControllerUpdater;

        [Inject] private ICreatureSpawnPointRepository _creatureSpawnPointRepository;
        [Inject] private ICreatureSpawnController _creatureSpawnController;

        [Inject] private IGameLevelService _gameLevelService;
        [Inject] private IGameLevelSpawnController _gameLevelSpawnController;

        private bool _updateEnabled;

        private IGameLevelController _gameLevelController;

        public Task<bool> Init()
        {
            _creaturePrefabConfig.Init();

            _monoUpdater.Subscribe((IMonoFixedUpdatable)this);
            _monoUpdater.Subscribe((IMonoUpdatable)this);
            _monoUpdater.Subscribe((IMonoLateUpdatable)this);
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            _monoUpdater.UnSubscribe((IMonoFixedUpdatable)this);
            _monoUpdater.UnSubscribe((IMonoUpdatable)this);
            _monoUpdater.UnSubscribe((IMonoLateUpdatable)this);
            return true;
        }

        public async Task<bool> OnStart()
        {
            _updateEnabled = false;
            
            await _gameLevelSpawnController.SpawnGameLevel();
            _gameLevelController = _gameLevelService.CurrentGameLevelController;
            await _gameLevelController.OnStart();
            
            PreSpawnCreatures();
            return true;
        }

        public async Task<bool> OnLateStart()
        {
            await _gameLevelController.OnLateStart();
            
            _updateEnabled = true;
            return true;
        }

        public void OnFixedUpdate()
        {
            if (!_updateEnabled)
                return;

            _creatureControllerUpdater.OnFixedUpdate();
        }

        public void OnUpdate()
        {
            if (!_updateEnabled)
                return;

            _creatureControllerUpdater.OnUpdate();
        }

        public void OnLateUpdate()
        {
            if (!_updateEnabled)
                return;

            _creatureControllerUpdater.OnLateUpdate();
            _creatureControllerRepository.OnLateUpdate();
        }

        private void PreSpawnCreatures()
        {
            var creatureSpawnPoints = _creatureSpawnPointRepository.GetAll();

            foreach (var spawnPoint in creatureSpawnPoints)
            {
                var creatureId = spawnPoint.CreatureId;
                _creatureSpawnController.Spawn(creatureId, true);
            }
        }
    }
}