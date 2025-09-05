using System;
using System.Collections.Generic;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.JSONParse;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.SyncSync;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.BalanceStorage.Sync
{
    public interface IProjectBalanceStorageSyncBase : IProjectService
    {
    }

    public abstract class ProjectBalanceStorageSyncBase : IProjectBalanceStorageSyncBase
    {
        [Inject] private IBalanceJSONParser _balanceJsonParser;
        
        public bool Init()
        {
            _balanceJsonParser.Init();
            var result = InitBalanceStorages();
            _balanceJsonParser.Flush();
            return result;
        }

        public bool Flush()
        {
            return true;
        }
        
        private bool InitBalanceStorages()
        {
            var storages = GetStorages();

            var result = true;

            foreach (var storage in storages)
            {
                var initResult = InitStorage(storage);
                result = result && initResult;
            }

            return result;
        }

        private bool InitStorage(IProjectService balanceStorage)
        {
            var storageName = balanceStorage.GetType().Name;

            LogUtils.Info(this, $"[{storageName}] init START");

            try
            {
                var initResult = balanceStorage.Init();

                if (!initResult)
                    return initResult;
            }
            catch (Exception e)
            {
                LogUtils.Error(this, $"[{storageName}] init ERROR");
                LogUtils.Error(this, e.Message);
                LogUtils.Error(this, e.StackTrace);
            }

            LogUtils.Info(this, $"[{storageName}] init SUCCESS");
            return true;
        }

        protected abstract HashSet<IProjectService> GetStorages();
    }
}
