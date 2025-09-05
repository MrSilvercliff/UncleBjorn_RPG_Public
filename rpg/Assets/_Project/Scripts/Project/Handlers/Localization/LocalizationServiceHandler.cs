using _Project.Scripts.Project.UI.Events;
using Plugins.ZerglingUnityPlugins.Localization_Total_JSON.Scripts;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.EventBus.Async;

namespace _Project.Scripts.Project.Handlers.Localization
{
    public class LocalizationServiceHandler : ILocalizationServiceHandler
    {
        [Inject] private IEventBusAsync _eventBus;
        
        public void OnLanguageChanged()
        {
            var eve = new ProjectLanguageChangedUIEvent();
            _eventBus.Fire(eve);
        }
    }
}
