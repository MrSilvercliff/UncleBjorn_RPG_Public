using System.Threading.Tasks;
using _Project.Scripts.Project.Services.Balance.Models;
using Defective.JSON;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.BalanceStorage.Async;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.JSONParse;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace _Project.Scripts.Project.Services.Balance.Storages
{
    public interface IInteractrableObjectsBalanceStorage : IBalanceStorageConfigAsyncBase<IInteractableObjectsBalanceModel, InteractableObjectsBalanceModel>
    {
        public float InteractMaxRange { get; }
        public float PlayerRotateLookInteractBlockTreshold { get; }
        public float DoorInteractTimeSeconds { get; }
    }

    public class InteractrableObjectsBalanceStorage : BalanceStorageConfigAsyncBase<IInteractableObjectsBalanceModel, InteractableObjectsBalanceModel>, IInteractrableObjectsBalanceStorage
    {
        public float InteractMaxRange => _balanceModel.InteractMaxRange;
        public float PlayerRotateLookInteractBlockTreshold => _balanceModel.PlayerRotateLookInteractBlockTreshold;
        public float DoorInteractTimeSeconds => _balanceModel.DoorInteractTimeSeconds;
        
        protected override Task<bool> OnInit()
        {
            return Task.FromResult(true);
        }
        
        protected override void OnBalanceModelAdded(IInteractableObjectsBalanceModel balanceModel)
        {
        }
    }
}
