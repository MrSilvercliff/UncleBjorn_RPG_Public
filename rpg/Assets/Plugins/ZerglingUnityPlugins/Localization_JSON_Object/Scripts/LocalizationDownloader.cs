using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Defective.JSON;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Log;
using ZerglingUnityPlugins.Tools.Scripts.Utils;

#if UNITY_EDITOR

namespace Plugins.ZerglingUnityPlugins.Localization_JSON_Object.Scripts
{
    public interface ILocalizationDownloader
    {
        void Setup(ILocalizationDownloadConfig config);
        void Download();
    }

    public class LocalizationDownloader : ILocalizationDownloader
    {
        private const string _spreadsheetUrlFormat = "https://docs.google.com/spreadsheets/d/{0}/gviz/tq?tqx=out:csv&sheet=";

        private ILocalizationDownloadConfig _config;

        private string _url;
        private List<string> _languages;
        private Dictionary<string, JSONObject> _locales;

        private string _currentPageName;
        private string _pageText;

        public LocalizationDownloader()
        {
            _url = string.Empty;
            _languages = new List<string>();
            _locales = new Dictionary<string, JSONObject>();

            _currentPageName = string.Empty;
            _pageText = string.Empty;
        }

        public void Setup(ILocalizationDownloadConfig config)
        {
            _config = config;
        }

        public void Download()
        {
            _url = string.Format(_spreadsheetUrlFormat, _config.GoogleSpreadsheetKey);
            
            var coroutine = DownloadTask();

            while (coroutine.MoveNext())
            {
            }
        }

        private IEnumerator DownloadTask()
        {
            var pages = _config.Pages;

            for (int i = 0; i < pages.Count; i++)
            {
                _currentPageName = pages[i];

                var download = DownloadProcess();
                while (download.MoveNext())
                    yield return null;

                var parseLanguages = ParseLanguagesProcess();
                while (parseLanguages.MoveNext())
                    yield return null;

                var parsePage = ParsePageProcess();
                while (parsePage.MoveNext()) 
                    yield return null;
            }

            var writeFiles = WriteFiles();
            while (writeFiles.MoveNext())
                yield return null;
        }

        private IEnumerator DownloadProcess()
        {
            LogUtils.Info(this, $"{_currentPageName} : Downloading...");

            var pageUrl = $"{_url}{_currentPageName}";
            var www = new WWW(pageUrl);

            while (www.MoveNext() && !www.isDone)
                yield return null;

            _pageText = www.text;
        }

        private IEnumerator ParseLanguagesProcess()
        {
            if (_languages.Count > 0)
                yield break;

            LogUtils.Info(this, $"{_currentPageName} : Parse languages...");

            var languagesCoroutine = ParseLanguagesTask();
            while (languagesCoroutine.MoveNext())
                yield return null;
        }

        private IEnumerator ParsePageProcess()
        {
            LogUtils.Info(this, $"{_currentPageName} : Parsing...");

            var parseCoroutine = ParsePageTask();

            while (parseCoroutine.MoveNext())
                yield return null;
        }

        private IEnumerator WriteFiles()
        {
            LogUtils.Info(this, $"Writing JSONs...");

            var projectFolderPath = DirectoryUtils.TryCreateDirectory(Application.dataPath, "_Project");
            var resourcesFolderPath = DirectoryUtils.TryCreateDirectory(projectFolderPath, "Resources");
            var localesFolderPath = DirectoryUtils.TryCreateDirectory(resourcesFolderPath, "Locales");

            foreach (var locale in _locales)
            {
                var language = locale.Key;
                var json = locale.Value;
                var filePath = $"{localesFolderPath}/Locale_{language}.json";
                File.WriteAllText(filePath, json.ToString(true));
                yield return null;
            }

            LogUtils.Error(this, "DONE!");
        }

        private IEnumerator ParseLanguagesTask()
        {
            var lines = _pageText.Split('\n');

            // дергаем лайн с языками из таблицы
            var languageLine = lines[0];
            var languages = Split(languageLine);

            // заполняем словарь пустыми жсонами, если еще это не сделали
            // с 1, потому что 0 стоит заголовок "Keys"
            for (int i = 1; i < languages.Length; i++)
            {
                var language = languages[i];
                if (string.IsNullOrEmpty(language))
                    continue;

                if (_locales.ContainsKey(language))
                    continue;

                _languages.Add(language);
                var json = JSONObject.Create(JSONObject.Type.Array);
                _locales[language] = json;
            }

            yield return null;
        }

        private IEnumerator ParsePageTask()
        {
            var keyString = "Key";
            var valueString = "Value";

            var lines = _pageText.Split('\n');

            // парсим конкретный лайн с ключом и значениями на всех языках
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                var lineInfo = Split(line);
                var key = lineInfo[0];

                if (_config.ParsePageNameToKey)
                    key = $"{_currentPageName}/{key}";

                for (int j = 0; j < _languages.Count; j++)
                {
                    var json = JSONObject.Create();
                    json.AddField(keyString, key);

                    var language = _languages[j];
                    if (j >= lineInfo.Length)
                    {
                        json.AddField(valueString, string.Empty);
                        _locales[language].Add(json);
                        continue;
                    }

                    var localeString = lineInfo[j + 1];
                    json.AddField(valueString, localeString);
                    _locales[language].Add(json);
                }
            }

            yield return null;
        }

        private string[] Split(string str)
        {
            return str.Substring(1, str.Length - 2).Split(new[] { "\",\"" }, StringSplitOptions.None);
        }
    }
}

#endif
