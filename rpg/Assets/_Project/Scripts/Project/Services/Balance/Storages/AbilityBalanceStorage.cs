using _Project.Scripts.Project.Services.Balance.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.BalanceStorage.Async;
using UnityEngine;

namespace _Project.Scripts.Project.Services.Balance.Storages
{
    public interface IAbilityBalanceStorage : IBalanceStorageDictionaryAsyncBase<IAbilityBalanceModel, AbilityBalanceModel>
    { 
    }

    public class AbilityBalanceStorage : BalanceStorageDictionaryAsyncBase<IAbilityBalanceModel, AbilityBalanceModel>, IAbilityBalanceStorage
    {
        protected override Task<bool> OnInit()
        {
            return Task.FromResult(true);
        }
        
        protected override void OnBalanceModelAdded(IAbilityBalanceModel balanceModel)
        {
        }
    }
}