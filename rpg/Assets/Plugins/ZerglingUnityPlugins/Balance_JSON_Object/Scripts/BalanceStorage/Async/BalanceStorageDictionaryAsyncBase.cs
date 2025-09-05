using System.Collections.Generic;
using System.Threading.Tasks;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.JSONParse;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.BalanceStorage.Async
{
    public interface IBalanceStorageDictionaryAsyncBase<TInterface, TClass> : IProjectSerivce
        where TInterface : IBalanceModelWithIdBase
        where TClass : class, TInterface, new()
    {
        IReadOnlyDictionary<string, TInterface> GetAllAsDicitonary();
        IReadOnlyList<TInterface> GetAllAsList();

        bool TryGetById(string id, out TInterface result);
    }

    public abstract class BalanceStorageDictionaryAsyncBase<TInterface, TClass> : IBalanceStorageDictionaryAsyncBase<TInterface, TClass> 
        where TInterface : IBalanceModelWithIdBase 
        where TClass : class, TInterface, new()
    {
        [Inject] private IBalanceJSONParser _balanceJsonParser;
        
        protected Dictionary<string, TInterface> _balanceModelsDict;
        protected List<TInterface> _balanceModelsList;
        
        public BalanceStorageDictionaryAsyncBase()
        {
            _balanceModelsDict = new Dictionary<string, TInterface>();
            _balanceModelsList = new List<TInterface>();
        }

        public async Task<bool> Init()
        {
            await InitBalanceModels();
            
            var balanceStorageType = GetType();
            if (_balanceJsonParser.IsDebugPrintNeeded(balanceStorageType))
                DebugPrint();
            
            await OnInit();
            return true;
        }

        public bool Flush()
        {
            return true;
        }

        private Task<bool> InitBalanceModels()
        {
            var balanceStorageType = GetType();
            var balanceModelsList = _balanceJsonParser.ParseBalanceDictionary<TInterface, TClass>(balanceStorageType);

            foreach (var balanceModel in balanceModelsList)
            {
                var id = balanceModel.Id;
                _balanceModelsDict[id] = balanceModel;
                _balanceModelsList.Add(balanceModel);
                OnBalanceModelAdded(balanceModel);
            }
            
            return Task.FromResult(true);
        }

        protected abstract void OnBalanceModelAdded(TInterface balanceModel);

        protected abstract Task<bool> OnInit();

        public IReadOnlyDictionary<string, TInterface> GetAllAsDicitonary()
        {
            return _balanceModelsDict;
        }

        public IReadOnlyList<TInterface> GetAllAsList()
        {
            return _balanceModelsList;
        }

        public bool TryGetById(string id, out TInterface result)
        {
            var tryGetResult = _balanceModelsDict.TryGetValue(id, out result);

            if (tryGetResult == false)
                LogUtils.Error(this, $"Balance model with id [{id}] DOES NOT EXIST!");

            return tryGetResult;
        }

        private void DebugPrint()
        {
            LogUtils.Error(this, $"========== DEBUG PRINT START =====");

            foreach (var balanceModel in _balanceModelsList)
                balanceModel.DebugPrint();
            
            LogUtils.Error(this, $"========== DEBUG PRINT END =====");
        }
    }
}
