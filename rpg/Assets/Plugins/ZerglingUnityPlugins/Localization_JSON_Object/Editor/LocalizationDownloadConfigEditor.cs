using Plugins.ZerglingUnityPlugins.Localization_JSON_Object.Scripts;
using Plugins.ZerglingUnityPlugins.Localization_Total_JSON.Scripts;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace Plugins.ZerglingUnityPlugins.Localization_JSON_Object.Editor
{
    [CustomEditor(typeof(LocalizationDownloadConfig))]
    public class LocalizationDownloadConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("DOWNLOAD"))
                OnDownloadButtonClick();

            if (GUILayout.Button("TEST"))
                OnTestButtonClick();
        }

        private void OnDownloadButtonClick()
        {
            var config = (ILocalizationDownloadConfig)target;
            config.Download();
        }

        private void OnTestButtonClick()
        { 
            var config = (ILocalizationDownloadConfig)target;
            config.Test();
        }
    }
}

#endif
