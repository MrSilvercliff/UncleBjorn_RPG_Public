using _Project.Scripts.Project.Enums;
using _Project.Scripts.Project.Scenes;
using _Project.Scripts.Project.Services.SceneLoading;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.StartScene.Scene
{
    public class StartSceneController : SceneController
    {
        [Inject] private IStartSceneServiceIniter _serviceIniter;

        [Inject] private ISceneLoadController _sceneLoadController;

        protected override async Task OnAwake()
        {
            await _serviceIniter.Init();
        }

        protected override async Task OnStart()
        {
            await _serviceIniter.InitServices(1);

            await _serviceIniter.InitServices(2);

            var sceneName = SceneName.MainScene.ToString();
            _sceneLoadController.LoadScene(sceneName);
        }

        protected override async Task OnLateStart()
        {
        }

        protected override void OnFlush()
        {
            _serviceIniter.Flush();
        }
    }
}