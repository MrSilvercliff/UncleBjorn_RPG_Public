using System;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Configs;

namespace Plugins.ZerglingUnityPlugins.Localization_Total_JSON.Scripts
{
    public interface ILocalizationConfig : IConfigBase
    {
        SystemLanguage DefaultLanguage { get; }
        IReadOnlyList<SystemLanguage> GetLanguagesList();
        TextAsset GetLocaleFile(SystemLanguage language);
    }

    [CreateAssetMenu(fileName = "LocalizationConfig", menuName = "Zergling plugins/Localization/LocalizationConfig")]
    public class LocalizationConfig : ConfigBase<LocalizationConfig>, ILocalizationConfig
    {
        public SystemLanguage DefaultLanguage => _defaultLanguage;

        [SerializeField] private SystemLanguage _defaultLanguage;
        [SerializeField] private LocalizationConfigItem[] _configItems;

        private Dictionary<SystemLanguage, TextAsset> _localeFilesDict;

        public override void Init()
        {
            base.Init();
            _localeFilesDict = new Dictionary<SystemLanguage, TextAsset>();

            for (int i = 0; i < _configItems.Length; i++)
            {
                var configItem = _configItems[i];
                var language = configItem.Language;
                var localeFile = configItem.LocaleFile;
                _localeFilesDict[language] = localeFile;
            }
        }

        public IReadOnlyList<SystemLanguage> GetLanguagesList()
        {
            var result = new List<SystemLanguage>();

            for (int i = 0; i < _configItems.Length; i++)
            {
                var configItem = _configItems[i];
                result.Add(configItem.Language);
            }

            return result;
        }

        public TextAsset GetLocaleFile(SystemLanguage language)
        {
            if (!_localeFilesDict.ContainsKey(language))
                return _localeFilesDict[_defaultLanguage];

            return _localeFilesDict[language];
        }
    }

    [Serializable]
    public class LocalizationConfigItem
    {
        public SystemLanguage Language => _language;
        public TextAsset LocaleFile => _localeFile;

        [SerializeField] private SystemLanguage _language;
        [SerializeField] private TextAsset _localeFile;
    }
}


