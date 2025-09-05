using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.Async;
using ZerglingUnityPlugins.Tools.Scripts.Mono;

namespace _Project.Scripts.GameScene.Services.Creatures
{
    public interface ICreatureControllerUpdater : ILateStartable, IMonoUpdatable, IMonoFixedUpdatable, IMonoLateUpdatable
    {
    }

    public class CreatureControllerUpdater : ICreatureControllerUpdater
    {
        [Inject] private ICreatureControllerRepository _repository;

        public async Task<bool> OnLateStart()
        {
            var controllers = _repository.GetAll();

            foreach (var controller in controllers)
                controller.CreatureStateMachine.EnterState(controller.OnSpawnState);

            return true;
        }

        public void OnFixedUpdate()
        {
            var controllers = _repository.GetAll();

            foreach (var controller in controllers)
                controller.OnFixedUpdate();
        }

        public void OnUpdate()
        {
            var controllers = _repository.GetAll();

            foreach (var controller in controllers)
                controller.OnUpdate();
        }

        public void OnLateUpdate()
        {
            var controllers = _repository.GetAll();

            foreach (var controller in controllers)
                controller.OnLateUpdate();
        }
    }
}