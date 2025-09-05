using _Project.Scripts.Project.UI.Events;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.EventBus.Async;

namespace _Project.Scripts.Project.UI.Components
{
    public class TextMeshLocalizationComponent : MonoBehaviour
    {
        [SerializeField] private string _key;

        [Inject] private IEventBusAsync _eventBus;
        // [Inject] private ILocalizationService _localizationService;

        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            _eventBus.Subscribe<ProjectLanguageChangedUIEvent>(OnProjectLanguageChangedUIEvent);
            ApplyLocalization();
        }

        private void OnDisable()
        {
            _eventBus.UnSubscribe<ProjectLanguageChangedUIEvent>(OnProjectLanguageChangedUIEvent);
        }

        private void ApplyLocalization()
        {
            // var localeText = _localizationService.Localize(_key);
            // _text.text = localeText;
        }

        private async Task OnProjectLanguageChangedUIEvent(ProjectLanguageChangedUIEvent eve)
        {
            ApplyLocalization();
        }
    }
}