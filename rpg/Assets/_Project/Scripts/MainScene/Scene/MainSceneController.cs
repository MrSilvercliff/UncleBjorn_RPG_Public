using _Project.Scripts.Project.Scenes;
using _Project.Scripts.Project.UI.Views.MainMenu;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Views;

namespace _Project.Scripts.MainScene.Scene
{
    public class MainSceneController : SceneController
    {
        [Inject] private IMainSceneServiceIniter _serviceIniter;

        [Inject] private IViewController _viewController;

        protected override async Task OnAwake()
        {
            await _serviceIniter.Init();
        }

        protected override async Task OnStart()
        {
            await _serviceIniter.InitServices(1);

            await _viewController.OpenView<MainMenu>();
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