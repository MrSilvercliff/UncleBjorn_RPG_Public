using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.ZenjectExtentions.SceneInstallers;

namespace _Project.Scripts.MainScene.Scene
{
    public class MainSceneInstaller : SceneInstallerBasic
    {
        protected override void OnInstallBindings()
        {
            BindSceneServiceIniter();
        }

        private void BindSceneServiceIniter()
        {
            Container.Bind<IMainSceneServiceIniter>().To<MainSceneServiceIniter>().AsSingle();
        }
    }
}