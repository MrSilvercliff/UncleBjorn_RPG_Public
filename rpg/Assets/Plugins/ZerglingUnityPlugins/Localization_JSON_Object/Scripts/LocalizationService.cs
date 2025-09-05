using System.Collections.Generic;
using System.Threading.Tasks;
using Defective.JSON;
using Plugins.ZerglingUnityPlugins.Localization_Total_JSON.Scripts;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;
using ZerglingUnityPlugins.Tools.Scripts.Utils;

namespace Plugins.ZerglingUnityPlugins.Localization_JSON_Object.Scripts
{
    public interface ILocalizationService : IProjectSerivce
    {
        SystemLanguage CurrentLanguage { get; }
        
        IReadOnlyList<SystemLanguage> AvailableLanguages { get; }
        string Localize(string key);
        void ChangeLanguage(SystemLanguage language);
    }

    public class LocalizationService : ILocalizationService
    {
        public SystemLanguage CurrentLanguage => _currentLanguage;
        public IReadOnlyList<SystemLanguage> AvailableLanguages => _availableLanguages;

        [Inject] private ILocalizationConfig _config;
        [Inject] private ILocalizationServiceHandler _serviceHandler;

        private const string GAME_LANGUAGE_PLAYER_PREF = "GameLanguage";

        private List<SystemLanguage> _availableLanguages;

        private SystemLanguage _currentLanguage;
        private Dictionary<string, string> _currentLanguageLocale;

        public LocalizationService()
        {
            _currentLanguageLocale = new Dictionary<string, string>();
        }

        public Task<bool> Init()
        {
            _config.Init();
            _availableLanguages = (List<SystemLanguage>)_config.GetLanguagesList();
            LoadLanguage();
            FillCurrentLocale();
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            return true;
        }

        private void LoadLanguage()
        {
            if (PlayerPrefs.HasKey(GAME_LANGUAGE_PLAYER_PREF))
            {
                var language = PlayerPrefs.GetString(GAME_LANGUAGE_PLAYER_PREF);
                _currentLanguage = StringUtils.ParseEnum(language, SystemLanguage.English);
            }
            else
            {
                var systemLanguage = Application.systemLanguage;

                if (systemLanguage == SystemLanguage.Unknown || !_availableLanguages.Contains(systemLanguage))
                    _currentLanguage = _config.DefaultLanguage;

                PlayerPrefs.SetString(GAME_LANGUAGE_PLAYER_PREF, _currentLanguage.ToString());
            }
        }

        private void FillCurrentLocale()
        {
            _currentLanguageLocale.Clear();

            var file = _config.GetLocaleFile(_currentLanguage);
            var fileText = file.text;
            var json = new JSONObject(fileText);

            for (int i = 0; i < json.list.Count; i++)
            {
                var item = json.list[i];
                var key = item["Key"].stringValue;
                var value = item["Value"].stringValue;
                _currentLanguageLocale[key] = value;
            }
        }

        public string Localize(string key)
        {
            var tryGetResult = _currentLanguageLocale.TryGetValue(key, out var result);

            if (!tryGetResult)
                return $"???{key}???";

            if (string.IsNullOrEmpty(result))
                return $"???{key}???";

            return result;
        }

        public void ChangeLanguage(SystemLanguage language)
        {
            if (language == _currentLanguage)
                return;

            _currentLanguage = language;
            PlayerPrefs.SetString(GAME_LANGUAGE_PLAYER_PREF, _currentLanguage.ToString());
            FillCurrentLocale();
            _serviceHandler.OnLanguageChanged();
        }
    }
}
