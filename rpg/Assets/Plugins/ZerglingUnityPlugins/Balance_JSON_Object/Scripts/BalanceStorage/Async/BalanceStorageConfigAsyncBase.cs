using System.Threading.Tasks;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.JSONParse;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.BalanceStorage.Async
{
    public interface IBalanceStorageConfigAsyncBase<TInterface, TClass> : IProjectSerivce
        where TInterface : IBalanceModelBase
        where TClass : class, TInterface, new()
    {
    }

    public abstract class BalanceStorageConfigAsyncBase<TInterface, TClass> : IBalanceStorageConfigAsyncBase<TInterface, TClass> 
        where TInterface : IBalanceModelBase 
        where TClass : class, TInterface, new()
    {
        [Inject] private IBalanceJSONParser _balanceJsonParser;
        
        protected TInterface _balanceModel;

        public BalanceStorageConfigAsyncBase()
        {
            _balanceModel = default;
        }

        public async Task<bool> Init()
        {
            await InitBalanceModel();

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

        private Task<bool> InitBalanceModel()
        {
            var balanceStorageType = GetType();
            _balanceModel = _balanceJsonParser.ParseBalanceConfig<TInterface, TClass>(balanceStorageType);
            OnBalanceModelAdded(_balanceModel);
            return Task.FromResult(true);
        }

        protected abstract void OnBalanceModelAdded(TInterface balanceModel);

        protected abstract Task<bool> OnInit();

        private void DebugPrint()
        {
            LogUtils.Error(this, $"========== DEBUG PRINT START =====");
            
            _balanceModel.DebugPrint();
            
            LogUtils.Error(this, $"========== DEBUG PRINT END =====");
        }
    }
}
