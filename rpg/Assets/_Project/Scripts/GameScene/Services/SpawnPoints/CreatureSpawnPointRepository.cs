using _Project.Scripts.GameScene.SpawnPoints;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;
using ZerglingUnityPlugins.Tools.Scripts.Repositories;

namespace _Project.Scripts.GameScene.Services.SpawnPoints
{
    public interface ICreatureSpawnPointRepository : IRepositoryBase<ICreatureSpawnPointController>, IProjectSerivce
    {
        bool TryGetFirstByCreatureId(string creatureId, out ICreatureSpawnPointController spawnPointController);
    }

    public class CreatureSpawnPointRepository : ICreatureSpawnPointRepository
    {
        public int Count => _allSpawnPoints.Count;

        private List<ICreatureSpawnPointController> _allSpawnPoints;
        private Dictionary<string, List<ICreatureSpawnPointController>> _byCreatureId;

        public CreatureSpawnPointRepository()
        {
            _allSpawnPoints = new();
            _byCreatureId = new();
        }

        public Task<bool> Init()
        {
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            foreach (var byCreatureIdList in _byCreatureId.Values)
                byCreatureIdList.Clear();

            _byCreatureId.Clear();
            _allSpawnPoints.Clear();
            return true;
        }

        public void Add(ICreatureSpawnPointController item)
        {
            _allSpawnPoints.Add(item);
            AddByCreatureId(item);
        }

        private void AddByCreatureId(ICreatureSpawnPointController item)
        {
            var creatureId = item.CreatureId;

            if (!_byCreatureId.ContainsKey(creatureId))
                _byCreatureId[creatureId] = new();

            var list = _byCreatureId[creatureId];
            list.Add(item);
        }

        public IReadOnlyCollection<ICreatureSpawnPointController> GetAll()
        {
            return _allSpawnPoints;
        }

        public bool TryGetFirstByCreatureId(string creatureId, out ICreatureSpawnPointController spawnPointController)
        {
            var result = _byCreatureId.TryGetValue(creatureId, out var list);
            spawnPointController = null;

            if (result)
                spawnPointController = list[0];

            return result;
        }
    }
}