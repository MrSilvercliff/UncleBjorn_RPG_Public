using _Project.Scripts.GameScene.Creatures.Basis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.ZenjectExtentions.ContextProvider;

namespace _Project.Scripts.GameScene.Creatures.Basis.Prefab
{
    public class CreaturePrefabFactory : IFactory<CreaturePrefab, CreaturePrefab>
    {
        [Inject] private IZenjectContextProvider _contextProvider;

        public CreaturePrefab Create(CreaturePrefab param)
        {
            var container = _contextProvider.Container;
            var result = container.InstantiatePrefabForComponent<CreaturePrefab>(param);
            return result;
        }
    }
}