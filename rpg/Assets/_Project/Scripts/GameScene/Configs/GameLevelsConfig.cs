using System;
using System.Collections.Generic;
using _Project.Scripts.GameScene.Enums;
using _Project.Scripts.GameScene.Services.GameLevels;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Configs;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace _Project.Scripts.GameScene.Configs
{
    public interface IGameLevelsConfig : IConfigBase
    {
        bool LoadDevelopLevel { get; }
        GameLevelId DevelopLevelId { get; }
        GameLevelId NewGameLevelId { get; }

        GameObject GetGameObjectById(GameLevelId gameLevelId);
    }

    [CreateAssetMenu(fileName = "GameLevelsConfig", menuName = "Project/Configs/Game Scene/Game Levels Config")]
    public class GameLevelsConfig : ScriptableObject, IGameLevelsConfig
    {
        public bool LoadDevelopLevel => _loadDevelopLevel;
        public GameLevelId DevelopLevelId => _developLevelId;
        public GameLevelId NewGameLevelId => _newGameLevelId;

        [SerializeField] private bool _loadDevelopLevel;
        [SerializeField] private GameLevelId _developLevelId;
        [SerializeField] private GameLevelId _newGameLevelId;
        [SerializeField] private GameLevelConfigItem[] _gameLevels;

        private Dictionary<GameLevelId, GameObject> _gameObjectsById;
        
        public void Init()
        {
            _gameObjectsById = new();

            foreach (var gameLevelConfigItem in _gameLevels)
            {
                var id = gameLevelConfigItem.GameLevelId;
                var controller = gameLevelConfigItem.LevelGameObject;
                _gameObjectsById[id] = controller;
            }
        }
        
        public GameObject GetGameObjectById(GameLevelId gameLevelId)
        {
            if (_gameObjectsById.TryGetValue(gameLevelId, out var gameObject))
                return gameObject;
            
            LogUtils.Error(this, $"Game level [{gameLevelId}] does not exist!");
            return _gameObjectsById[_developLevelId];
        }
    }

    [Serializable]
    public class GameLevelConfigItem
    {
        public GameLevelId GameLevelId => _gameLevelId;
        public GameObject LevelGameObject => _levelGameObject;
        
        [SerializeField] private GameLevelId _gameLevelId;
        [SerializeField] private GameObject _levelGameObject;
    }
}
