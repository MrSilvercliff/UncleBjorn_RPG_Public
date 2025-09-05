using _Project.Scripts.GameScene.Creatures.Basis;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Scripts.Project.Enums;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;
using ZerglingUnityPlugins.Tools.Scripts.Log;
using ZerglingUnityPlugins.Tools.Scripts.Mono;
using ZerglingUnityPlugins.Tools.Scripts.Repositories;

namespace _Project.Scripts.GameScene.Services.Creatures
{
    public interface ICreatureControllerRepository : IRepositoryBase<ICreatureController>, IProjectSerivce, IMonoLateUpdatable
    {
        IReadOnlyDictionary<int, ICreatureController> CreatureControllerDictionary { get; }
        
        ICreatureController PlayerController { get; }

        bool TryGetByInstanceId(int instanceId, out ICreatureController creatureController);

        bool Remove(int instanceID);
        bool Remove(ICreatureController creatureController);
    }

    public class CreatureControllerRepository : ICreatureControllerRepository
    {
        public IReadOnlyDictionary<int, ICreatureController> CreatureControllerDictionary => _activeCreatureControllersDict;
        public int Count => _activeCreatureControllersHashSet.Count;

        public ICreatureController PlayerController => _playerController;

        private Dictionary<int, ICreatureController> _activeCreatureControllersDict;
        private HashSet<ICreatureController> _activeCreatureControllersHashSet;

        private HashSet<ICreatureController> _toAddCreatureControllers;
        private HashSet<ICreatureController> _toRemoveCreatureControllers;

        private ICreatureController _playerController;

        public Task<bool> Init()
        {
            _activeCreatureControllersDict = new Dictionary<int, ICreatureController>();
            _activeCreatureControllersHashSet = new HashSet<ICreatureController>();

            _toAddCreatureControllers = new HashSet<ICreatureController>();
            _toRemoveCreatureControllers = new HashSet<ICreatureController>();
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            return true;
        }

        public void Add(ICreatureController item)
        {
            _toAddCreatureControllers.Add(item);
        }

        public bool Remove(int instanceID)
        {
            var tryGetResult = _activeCreatureControllersDict.TryGetValue(instanceID, out var controller);

            if (!tryGetResult)
                return false;

            _toRemoveCreatureControllers.Add(controller);

            return true;
        }

        public bool Remove(ICreatureController creatureController)
        {
            _toRemoveCreatureControllers.Add(creatureController);

            return true;
        }

        public bool TryGetByInstanceId(int instanceId, out ICreatureController creatureController)
        {
            var result = _activeCreatureControllersDict.TryGetValue(instanceId, out creatureController);

            if (!result)
                LogUtils.Error(this, $"Creature controller with instance id [{instanceId}] not found!");

            return result;
        }

        public IReadOnlyCollection<ICreatureController> GetAll()
        {
            return _activeCreatureControllersHashSet;
        }

        public void OnLateUpdate()
        {
            CheckToRemoveCreatureControllers();
            CheckToAddCreatureControllers();
        }

        private void CheckToAddCreatureControllers()
        {
            foreach (var creatureController in _toAddCreatureControllers)
            {
                var instanceId = creatureController.InstanceID;
                _activeCreatureControllersDict[instanceId] = creatureController;
                _activeCreatureControllersHashSet.Add(creatureController);

                if (creatureController.CreatureType == CreatureType.PLAYER)
                    _playerController = creatureController;
            }
        }

        private void CheckToRemoveCreatureControllers()
        {
            foreach (var creatureController in _toRemoveCreatureControllers)
            {
                var instanceId = creatureController.InstanceID;
                _activeCreatureControllersDict.Remove(instanceId);
                _activeCreatureControllersHashSet.Remove(creatureController);
                
                if (creatureController.CreatureType == CreatureType.PLAYER)
                    _playerController = null;
            }
        }
    }
}