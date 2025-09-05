using _Project.Scripts.GameScene.Enums;
using _Project.Scripts.GameScene.SpawnPoints;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;
using ZerglingUnityPlugins.Tools.Scripts.Repositories;

namespace _Project.Scripts.GameScene.Services.SpawnPoints
{
    public interface ISpawnPointRepository : IRepositoryBase<ISpawnPointController>, IProjectSerivce
    { 
    }

    public class SpawnPointRepository : ISpawnPointRepository
    {
        public int Count => _allSpawnPoints.Count;

        private List<ISpawnPointController> _allSpawnPoints;
        private Dictionary<SpawnPointType, List<ISpawnPointController>> _bySpawnPointType;

        public SpawnPointRepository()
        {
            _allSpawnPoints = new();
            _bySpawnPointType = new();
        }

        public Task<bool> Init()
        {
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            foreach (var bySpawnPointTypeList in _bySpawnPointType.Values)
                bySpawnPointTypeList.Clear();

            _bySpawnPointType.Clear();
            _allSpawnPoints.Clear();
            return true;
        }

        public void Add(ISpawnPointController item)
        {
            _allSpawnPoints.Add(item);
            AddByType(item);
        }

        private void AddByType(ISpawnPointController item)
        {
            var spawnPointType = item.SpawnPointType;

            if (!_bySpawnPointType.ContainsKey(spawnPointType))
                _bySpawnPointType[spawnPointType] = new();

            var list = _bySpawnPointType[spawnPointType];
            list.Add(item);
        }

        public IReadOnlyCollection<ISpawnPointController> GetAll()
        {
            return _allSpawnPoints;
        }
    }
}