using _Project.Scripts.GameScene.Creatures.Basis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Configs;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace _Project.Scripts.GameScene.Configs
{
    public interface ICreaturePrefabConfig : IConfigBase
    {
        CreaturePrefab Get(string id);
    }

    [CreateAssetMenu(fileName = "CreaturePrefabConfig", menuName = "Project/Configs/Game Scene/Creature Prefab Config")]
    public class CreaturePrefabConfig : ScriptableObject, ICreaturePrefabConfig
    {
        [SerializeField] private CreaturePrefab[] _prefabsList;

        private Dictionary<string, CreaturePrefab> _prefabsDict;

        public void Init()
        {
            _prefabsDict = new();

            for (int i = 0; i < _prefabsList.Length; i++)
            {
                var prefab = _prefabsList[i];

                var id = prefab.Id;
                _prefabsDict[id] = prefab;
            }
        }

        public CreaturePrefab Get(string id)
        {
            var tryGetResult = _prefabsDict.TryGetValue(id, out var result);

            if (!tryGetResult)
                LogUtils.Error(this, $"Creature prefab with id [{id}] does not exist!");

            return result;
        }
    }
}