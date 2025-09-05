using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.ZenjectExtentions.SceneInstallers;

namespace _Project.Scripts.StartScene.Scene
{
    public class StartSceneInstaller : SceneInstallerBasic
    {
        protected override void OnInstallBindings()
        {
            Container.Bind<IStartSceneServiceIniter>().To<StartSceneServiceIniter>().AsSingle();
        }
    }
}