using _Project.Scripts.Project.Monobeh;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.Async;

namespace _Project.Scripts.GameScene.SpawnPoints
{
    public class SpawnPointContainer : ProjectMonoBehaviour, IStartable
    {
        public async Task<bool> OnStart()
        {
            await InvokeSpawnPointsOnStart();
            return true;
        }

        private async Task InvokeSpawnPointsOnStart()
        {
            var spawnPoints = Transform.GetComponentsInChildren<SpawnPointController>();

            foreach (var spawnPoint in spawnPoints)
                await spawnPoint.OnStart();
        }
    }
}