using _Project.Scripts.GameScene.Abilities;
using _Project.Scripts.GameScene.Configs;
using _Project.Scripts.GameScene.Creatures;
using _Project.Scripts.GameScene.Creatures.Basis;
using _Project.Scripts.GameScene.Creatures.Basis.Prefab;
using _Project.Scripts.GameScene.Creatures.Player;
using _Project.Scripts.GameScene.Creatures.Player.States;
using _Project.Scripts.GameScene.GameLoop;
using _Project.Scripts.GameScene.Services.Abilities;
using _Project.Scripts.GameScene.Services.Creatures;
using _Project.Scripts.GameScene.Services.Input;
using _Project.Scripts.GameScene.Services.InteractableObjects;
using _Project.Scripts.GameScene.Services.SpawnPoints;
using _Project.Scripts.GameScene.Services.StateMachines;
using _Project.Scripts.Project.Services.Balance.Models;
using _Project.Scripts.Project.Services.StateMachines;
using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.GameScene.Services.GameLevels;
using UnityEngine;
using ZerglingUnityPlugins.ZenjectExtentions.SceneInstallers;

namespace _Project.Scripts.GameScene.Scene
{
    public class GameSceneInstaller : SceneInstallerBasic
    {
        [SerializeField] private GameSceneObjectPoolContainers _objectPoolContainers;

        [Header("CONFIGS")] 
        [SerializeField] private CreaturePrefabConfig _creaturePrefabConfig;
        [SerializeField] private GameLevelsConfig _gameLevelsConfig;

        [Header("INPUT")] [SerializeField] private UnityInputHandler _unityInputHandler;

        protected override void OnInstallBindings()
        {
            BindConfigs();

            BindInputServices();

            BindAbilityServices();

            BindCreatureServices();

            BindSpawnPointsServices();

            BindObjectPools();

            BindStateMachineServices();
            
            BindGameLevelServices();

            BindGameLoopServices();

            BindInteractableObjectServices();

            BindSceneServiceIniter();
        }

        private void BindSceneServiceIniter()
        {
            Container.Bind<IGameSceneServiceIniter>().To<GameSceneServiceIniter>().AsSingle();
        }

        private void BindConfigs()
        {
            Container.Bind<ICreaturePrefabConfig>().FromInstance(_creaturePrefabConfig).AsSingle();
            Container.Bind<IGameLevelsConfig>().FromInstance(_gameLevelsConfig).AsSingle();
        }

        private void BindInputServices()
        {
            Container.Bind<IInputController>().To<InputController>().AsSingle();
            Container.Bind<IInputHandler>().FromInstance(_unityInputHandler).AsSingle();
        }

        private void BindAbilityServices()
        {
            BindAbilityFactories();

            Container.Bind<IAbilitiesProvider>().To<AbilitiesProvider>().AsSingle();
        }

        private void BindAbilityFactories()
        {
            Container.BindFactory<IAbilityBalanceModel, AbilityBasicMove, AbilityBasicMove.Factory>();
        }

        private void BindCreatureServices()
        {
            BindCreatureStateFactories();

            Container.Bind<ICreatureHelper>().To<CreatureHelper>().AsSingle();

            Container.BindFactory<ICreatureBalanceModel, CreatureModel, CreatureModel.Factory>();

            Container.Bind<ICreatureModelCreator>().To<CreatureModelCreator>().AsSingle();

            Container.Bind<ICreatureSpawnController>().To<CreatureSpawnController>().AsSingle();

            Container.Bind<ICreatureControllerRepository>().To<CreatureControllerRepository>().AsSingle();
            Container.Bind<ICreatureControllerUpdater>().To<CreatureControllerUpdater>().AsSingle();
        }

        private void BindCreatureStateFactories()
        {
            BindPlayerStateFactories();
        }

        private void BindPlayerStateFactories()
        {
            Container.BindFactory<IPlayerController, PlayerStateIdle, PlayerStateIdle.Factory>();
            Container.BindFactory<IPlayerController, PlayerStateMove, PlayerStateMove.Factory>();
            Container.BindFactory<IPlayerController, PlayerStateFreeLook, PlayerStateFreeLook.Factory>();
            Container.BindFactory<IPlayerController, PlayerStateRotateLook, PlayerStateRotateLook.Factory>();
            Container.BindFactory<IPlayerController, PlayerStateFreeFall, PlayerStateFreeFall.Factory>();
            Container.BindFactory<IPlayerController, PlayerStateJump, PlayerStateJump.Factory>();

            Container.Bind<IPlayerStateCreator>().To<PlayerStateCreator>().AsSingle();
        }

        private void BindSpawnPointsServices()
        {
            Container.Bind<ISpawnPointRepository>().To<SpawnPointRepository>().AsSingle();
            Container.Bind<ICreatureSpawnPointRepository>().To<CreatureSpawnPointRepository>().AsSingle();
        }

        private void BindObjectPools()
        {
            Container.Bind<IGameSceneObjectPoolContainers>().FromInstance(_objectPoolContainers).AsSingle();
            
            BindCreatureObjectPools();
        }

        private void BindCreatureObjectPools()
        {
            BindPlayerControllerPool();
            BindCreaturePrefabPool();
        }

        private void BindPlayerControllerPool()
        {
            var poolItem = _objectPoolContainers.PlayerController;

            var prefab = poolItem.Prefab;
            var container = poolItem.Container;

            Container.BindMemoryPool<PlayerController, PlayerControllerPool>()
                .WithInitialSize(1)
                .FromComponentInNewPrefab(prefab)
                .UnderTransform(container);
        }

        private void BindCreaturePrefabPool()
        {
            Container.BindFactory<CreaturePrefab, CreaturePrefab, CreaturePrefab.Factory>().FromFactory<CreaturePrefabFactory>();

            Container.Bind<ICreaturePrefabPool>().To<CreaturePrefabPool>().AsSingle();
        }

        private void BindStateMachineServices()
        {
            Container.BindFactory<PlayerStateMachine, PlayerStateMachine.Factory>();
            Container.Bind<IStateMachineCreator>().To<GameSceneStateMachineCreator>().AsSingle();
        }

        private void BindGameLevelServices()
        {
            Container.Bind<IGameLevelService>().To<GameLevelService>().AsSingle();
            Container.Bind<IGameLevelSpawnController>().To<GameLevelSpawnController>().AsSingle();
        }

        private void BindGameLoopServices()
        {
            Container.Bind<IGameLoopController>().To<GameLoopController>().AsSingle();
        }

        private void BindInteractableObjectServices()
        {
            Container.Bind<IInteractableObjectService>().To<InteractableObjectService>().AsSingle();
        }
    }
}