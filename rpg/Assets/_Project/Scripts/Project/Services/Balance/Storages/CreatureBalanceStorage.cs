using _Project.Scripts.Project.Services.Balance.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.BalanceStorage.Async;
using UnityEngine;

namespace _Project.Scripts.Project.Services.Balance.Storages
{
    public interface ICreatureBalanceStorage : IBalanceStorageDictionaryAsyncBase<ICreatureBalanceModel, CreatureBalanceModel>
    { 
    }

    public class CreatureBalanceStorage : BalanceStorageDictionaryAsyncBase<ICreatureBalanceModel, CreatureBalanceModel>, ICreatureBalanceStorage
    {
        protected override Task<bool> OnInit()
        {
            return Task.FromResult(true);
        }
        
        protected override void OnBalanceModelAdded(ICreatureBalanceModel balanceModel)
        {
        }
    }
}