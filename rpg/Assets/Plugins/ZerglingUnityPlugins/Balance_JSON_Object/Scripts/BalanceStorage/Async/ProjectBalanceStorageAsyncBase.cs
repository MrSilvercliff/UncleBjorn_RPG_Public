using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.JSONParse;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;
using ZerglingUnityPlugins.Tools.Scripts.Log;
using ZerglingUnityPlugins.ZenjectExtentions.ContextProvider;

namespace Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.BalanceStorage.Async
{
    public interface IProjectBalanceStorageAsyncBase : IProjectSerivce
    {
    }

    public abstract class ProjectBalanceStorageAsyncBase : IProjectBalanceStorageAsyncBase
    {
        [Inject] private IBalanceJSONParser _balanceJsonParser;
        [Inject] private IZenjectContextProvider _zenjectContextProvider;
        
        public async Task<bool> Init()
        {
            _balanceJsonParser.Init();
            var result = await InitBalanceStorages();
            _balanceJsonParser.Flush();
            return result;
        }

        public bool Flush()
        {
            return true;
        }
        
        private async Task<bool> InitBalanceStorages()
        {
            var diContainer = _zenjectContextProvider.Container;
            var storages = GetStoragesToInit();

            var result = true;

            foreach (var storage in storages)
            {
                diContainer.Inject(storage);
                var initResult = await InitStorage(storage);
                result = result && initResult;
            }

            return result;
        }

        private async Task<bool> InitStorage(IProjectSerivce balanceStorage)
        {
            var storageName = balanceStorage.GetType().Name;

            LogUtils.Info(this, $"[{storageName}] init START");

            try
            {
                var initResult = await balanceStorage.Init();

                if (!initResult)
                    return false;
            }
            catch (Exception ex)
            {
                LogUtils.Error(this, $"[{storageName}] init ERROR");
                LogUtils.Error(this, ex.Message);
                LogUtils.Error(this, ex.StackTrace);
                return false;
            }

            LogUtils.Info(this, $"[{storageName}] init SUCCESS");
            return true;
        }

        protected abstract HashSet<IProjectSerivce> GetStoragesToInit();
    }
}
