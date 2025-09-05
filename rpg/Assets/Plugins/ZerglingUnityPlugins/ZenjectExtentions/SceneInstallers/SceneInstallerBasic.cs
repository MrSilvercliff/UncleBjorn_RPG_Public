using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Log;
using ZerglingUnityPlugins.ZenjectExtentions.ContextProvider;

namespace ZerglingUnityPlugins.ZenjectExtentions.SceneInstallers
{
    public class SceneInstallerBasic : MonoInstaller
    {
        [SerializeField] private SceneContext _sceneContext;

        [Inject] private IZenjectContextProvider _contextProvider;

        public override void InstallBindings()
        {
            LogUtils.Info(this, "Install bindings");
            OnInstallBindings();
            _contextProvider.UpdateContext(_sceneContext);
        }

        protected virtual void OnInstallBindings()
        {
        }
    }
}
