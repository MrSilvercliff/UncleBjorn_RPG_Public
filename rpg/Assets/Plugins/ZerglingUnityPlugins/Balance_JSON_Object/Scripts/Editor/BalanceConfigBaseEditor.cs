using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.Configs;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.Editor
{
    [CustomEditor(typeof(BalanceConfigBase), true)]
    public class BalanceConfigBaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("PARSE GOOGLE SHEET"))
                ParseBalance();
        }

        private void ParseBalance()
        {
            var config = (IBalanceConfig)target;
            config.ParseBalance();
        }
    }
}
#endif
