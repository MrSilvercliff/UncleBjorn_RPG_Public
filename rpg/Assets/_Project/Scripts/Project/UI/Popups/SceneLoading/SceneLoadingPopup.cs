using _Project.Scripts.Project.UI.Events;
using System.Threading.Tasks;
using _Project.Scripts.Project.UI.Data;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.EventBus.Async;
using ZerglingUnityPlugins.Tools.Scripts.Log;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Popups;

namespace _Project.Scripts.Project.UI.Popups.SceneLoading
{
    public class SceneLoadingPopup : ProjectPopupWindow
    {
        [Inject] private IEventBusAsync _eventBus;

        protected override Task OnPreOpen()
        {
            _eventBus.Subscribe<SceneLoadingProgressEvent>(OnSceneLoadingProgressEvent);
            return Task.FromResult(true);
        }

        protected override Task OnPreClose()
        {
            _eventBus.UnSubscribe<SceneLoadingProgressEvent>(OnSceneLoadingProgressEvent);
            return Task.FromResult(true);
        }

        protected override Task<bool> OnSetup(IWindowSetup setup)
        {
            return Task.FromResult(true);
        }

        private Task OnSceneLoadingProgressEvent(SceneLoadingProgressEvent eve)
        {
            LogUtils.Info(this, $"OnSceneLoadingProgressEvent");
            var progress = eve.Progress;
            (_windowUIData as SceneLoadingPopupUIData)?.SetProgress(progress);
            return Task.CompletedTask;
        }
    }
}