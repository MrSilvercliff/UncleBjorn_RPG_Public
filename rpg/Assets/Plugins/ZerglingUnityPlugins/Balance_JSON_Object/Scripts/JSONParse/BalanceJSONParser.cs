using System;
using System.Collections.Generic;
using Defective.JSON;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.BalanceStorage;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.Configs;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.SyncSync;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.JSONParse
{
    public interface IBalanceJSONParser : IProjectService
    {
        IReadOnlyList<TInterface> ParseBalanceDictionary<TInterface, TClass>(Type balanceStorageType) 
            where TInterface : IBalanceModelWithIdBase 
            where TClass : class, TInterface, new();

        TInterface ParseBalanceConfig<TInterface, TClass>(Type balanceStorageType)
            where TInterface : IBalanceModelBase
            where TClass : class, TInterface, new();

        bool IsDebugPrintNeeded(Type balanceStorageType);
    }

    public class BalanceJSONParser : IBalanceJSONParser
    {
        [Inject] private IBalanceConfig _config;
        [Inject] private IJSONParseHelper _jsonParseHelper;

        private Dictionary<string, JSONObject> _jsonSectionByBalanceStorageType;
        private Dictionary<string, bool> _debugPrintByBalanceStorageType;

        public BalanceJSONParser()
        {
            _jsonSectionByBalanceStorageType = new Dictionary<string, JSONObject>();
            _debugPrintByBalanceStorageType = new Dictionary<string, bool>();
        }

        public bool Init()
        {
            ParseJSON();
            return true;
        }

        public bool Flush()
        {
            _jsonSectionByBalanceStorageType.Clear();
            _jsonSectionByBalanceStorageType = null;
            
            _debugPrintByBalanceStorageType.Clear();
            _debugPrintByBalanceStorageType = null;
            return true;
        }

        private void ParseJSON()
        {
            var balanceFile = _config.BalanceFile;
            var json = JSONObject.Create(balanceFile.text);
            
            var balancePages = _config.GoogleSheetPages;

            foreach (var balancePage in balancePages)
            {
                if (balancePage.TargetBalanceStorage == null)
                    continue;
                
                var sectionKey = balancePage.PageName;
                var balanceStorageType = balancePage.TargetBalanceStorage.name;
                var debugPrintOnInit = balancePage.DebugPrintOnInit;
                
                var jsonSection = json[sectionKey];
                
                _jsonSectionByBalanceStorageType[balanceStorageType] = jsonSection;
                _debugPrintByBalanceStorageType[balanceStorageType] = debugPrintOnInit;
            }
        }
        
        public IReadOnlyList<TInterface> ParseBalanceDictionary<TInterface, TClass>(Type balanceStorageType) 
            where TInterface : IBalanceModelWithIdBase 
            where TClass : class, TInterface, new()
        {
            var jsonSection = _jsonSectionByBalanceStorageType[balanceStorageType.Name];
            
            var result = new List<TInterface>();

            foreach (var jsonItemKey in jsonSection.keys)
            {
                var jsonItem = jsonSection[jsonItemKey];
                var balanceModel = new TClass();

                var trySetupResult = balanceModel.TrySetup(jsonItem, _jsonParseHelper);

                if (!trySetupResult)
                {
                    LogUtils.Error(this, $"Cant parse JSON ITEM");
                    LogUtils.Error(this, jsonItem.ToString());
                    continue;
                }

                result.Add(balanceModel);
            }
            
            return result;
        }

        public TInterface ParseBalanceConfig<TInterface, TClass>(Type balanceStorageType) 
            where TInterface : IBalanceModelBase 
            where TClass : class, TInterface, new()
        {
            var jsonSection = _jsonSectionByBalanceStorageType[balanceStorageType.Name];
            var result = new TClass();
            
            var trySetupResult = result.TrySetup(jsonSection, _jsonParseHelper);

            if (!trySetupResult)
            {
                LogUtils.Error(this, $"Cant parse JSON SECTION");
                LogUtils.Error(this, jsonSection.ToString());
                return new TClass();
            }
            
            return result;
        }

        public bool IsDebugPrintNeeded(Type balanceStorageType)
        {
            return _debugPrintByBalanceStorageType[balanceStorageType.Name];
        }
    }
}
