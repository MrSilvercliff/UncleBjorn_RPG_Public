using Assets.Plugins.ZerglingUnityPlugins.Tools.Scripts.Mono;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Timers.Scripts;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;
using ZerglingUnityPlugins.Tools.Scripts.Mono;

namespace _Project.Scripts.Project.Services.Timers
{
    public interface ITimerController : ITimerControllerBase, IProjectSerivce, IMonoLateUpdatable
    {
    }

    public class TimerController : TimerControllerBase, ITimerController
    {
        [Inject] private IMonoUpdater _monoUpdater;

        public Task<bool> Init()
        {
            _monoUpdater.Subscribe(this);
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            return true;
        }

        public void OnLateUpdate()
        {
            var deltaTime = UnityEngine.Time.deltaTime;
            ProcessTimers(deltaTime);
        }
    }
}