using _Project.Scripts.GameScene.Enums;
using _Project.Scripts.GameScene.Services.SpawnPoints;
using _Project.Scripts.Project.Monobeh;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.Async;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace _Project.Scripts.GameScene.SpawnPoints
{
    public interface ISpawnPointController : IProjectMonoBehaviour, IStartable
    { 
        SpawnPointType SpawnPointType { get; }
    }

    public class SpawnPointController : ProjectMonoBehaviour, ISpawnPointController
    {
        public SpawnPointType SpawnPointType => _spawnPointType;

        [Header("SPAWN POINT CONTROLLER")]
        [SerializeField] private SpawnPointType _spawnPointType;

        [Inject] private ISpawnPointRepository _spawnPointRepository;

        public async Task<bool> OnStart()
        {
            DebugLog();
            OnStartProcess();
            return true;
        }

        private void DebugLog()
        {
            LogUtils.Info(gameObject.name, $"OnStart");
        }

        protected virtual void OnStartProcess()
        {
            _spawnPointRepository.Add(this);
        }
    }
}