using _Project.Scripts.GameScene.Creatures.Basis;
using _Project.Scripts.GameScene.Creatures.Basis.Prefab;
using _Project.Scripts.GameScene.Creatures.Player;
using _Project.Scripts.GameScene.Services.SpawnPoints;
using _Project.Scripts.GameScene.SpawnPoints;
using _Project.Scripts.Project.Enums;
using _Project.Scripts.Project.Services.Balance;
using _Project.Scripts.Project.Services.Balance.Models;
using _Project.Scripts.Project.Services.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace _Project.Scripts.GameScene.Services.Creatures
{
    public interface ICreatureSpawnController
    {
        void Spawn(string creatureId, bool enterOnSpawnState);
    }

    public class CreatureSpawnController : ICreatureSpawnController
    {
        [Inject] private IProjectBalanceStorage _balanceStorage;
        [Inject] private IStateMachineCreator _stateMachineCreator;

        [Inject] private ICreatureSpawnPointRepository _spawnPointRepository;
        [Inject] private ICreatureModelCreator _modelCreator;
        [Inject] private ICreaturePrefabPool _prefabPool;

        [Inject] private PlayerControllerPool _playerControllerPool;

        public void Spawn(string creatureId, bool enterOnSpawnState)
        {
            var creaturesBalanceStorage = _balanceStorage.Creatures;
            var tryGetResult = creaturesBalanceStorage.TryGetById(creatureId, out var creatureBalanceModel);

            if (!tryGetResult)
            {
                LogUtils.Error(this, $"Cant spawn creature with id [{creatureId}]");
                return;
            }

            var creatureType = creatureBalanceModel.CreatureType;

            switch (creatureType)
            {
                case CreatureType.PLAYER:
                    SpawnPlayer(creatureBalanceModel, enterOnSpawnState);
                    break;

                default:
                    LogUtils.Error(this, $"Spawn for creature type [{creatureType}] not implemented!");
                    break;
            }
        }

        private void SpawnPlayer(ICreatureBalanceModel balanceModel, bool enterOnSpawnState)
        {
            var creatureId = balanceModel.Id;

            var tryGetResult = TryGetFirstSpawnPoint(creatureId, out var spawnPoint);

            if (!tryGetResult)
            {
                LogUtils.Error(this, $"Cant spawn creature, spawn point doesnt exist!");
                return;
            }

            var prefabId = balanceModel.PrefabId;

            var controller = _playerControllerPool.Spawn();
            controller.Transform.position = spawnPoint.Transform.position;

            SetupCreatureModel(creatureId, controller);
            SetupStateMachine(controller);
            InitComponents(controller);
            SetupCreaturePrefab(prefabId, controller);
            InitStateMachineStateControllers(controller);
            TryEnterOnSpawnState(enterOnSpawnState, controller);
        }

        bool TryGetFirstSpawnPoint(string creatureId, out ICreatureSpawnPointController creatureSpawnPoint)
        {
            var tryGetResult = _spawnPointRepository.TryGetFirstByCreatureId(creatureId, out creatureSpawnPoint);
            return tryGetResult;
        }

        void SetupCreatureModel(string creatureId, ICreatureController creatureController)
        {
            var creatureModel = _modelCreator.GetCreatureModel(creatureId);
            creatureController.SetupModel(creatureModel);
        }

        void SetupStateMachine(ICreatureController creatureController)
        {
            var stateMachineType = creatureController.CreatureModel.StateMachineType;
            var stateMachine = (ICreatureStateMachine)_stateMachineCreator.CreateStateMachine(stateMachineType);
            creatureController.SetupStateMachine(stateMachine);
        }

        void SetupCreaturePrefab(string prefabId, ICreatureController creatureController)
        {
            var creaturePrefab = _prefabPool.Spawn(prefabId);
            creatureController.SetupPrefab(creaturePrefab);
        }

        void InitComponents(ICreatureController creatureController)
        {
            creatureController.InitComponents();
        }

        void InitStateMachineStateControllers(ICreatureController creatureController)
        {
            creatureController.CreatureStateMachine.InitStates();
        }

        void TryEnterOnSpawnState(bool enterOnSpawnState, ICreatureController creatureController)
        {
            if (!enterOnSpawnState)
                return;

            creatureController.CreatureStateMachine.EnterState(creatureController.OnSpawnState);
        }
    }
}