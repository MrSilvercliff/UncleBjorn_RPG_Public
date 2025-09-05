using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.Configs
{
    public interface IBalanceConfig
    {
        TextAsset BalanceFile { get; }
        string GoogleSheetId { get; }
        BalanceGoogleSheetPage[] GoogleSheetPages { get; }

        void ParseBalance();
    }

    /// <summary>
    /// не забудь в дочернем классе добавить
    /// [CreateAssetMenu(fileName = "T.Name", menuName = "Configs/T.Name")]
    /// чтобы добавить пункт меню для создания конфига
    /// </summary>
    public abstract class BalanceConfigBase : ScriptableObject, IBalanceConfig
    {
        public TextAsset BalanceFile => _balanceFile;
        public string GoogleSheetId => _googleSheetId;
        public BalanceGoogleSheetPage[] GoogleSheetPages => _googleSheetPages;

        [SerializeField] private TextAsset _balanceFile;
        [SerializeField] private string _googleSheetId;
        [SerializeField] private BalanceGoogleSheetPage[] _googleSheetPages;

        public abstract void ParseBalance();
    }

    [Serializable]
    public class BalanceGoogleSheetPage
    {
        public string PageName => _pageName;
        public Object TargetBalanceStorage => _targetBalanceStorage;
        public bool DebugPrintOnInit => _debugPrintOnInit;
        
        [SerializeField] private string _pageName;
        [SerializeField] private Object _targetBalanceStorage;
        [SerializeField] private bool _debugPrintOnInit;
    }
}
