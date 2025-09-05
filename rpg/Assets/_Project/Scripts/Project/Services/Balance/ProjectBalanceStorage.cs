using System.Collections.Generic;
using _Project.Scripts.Project.Services.Balance.Storages;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.BalanceStorage.Async;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;

namespace _Project.Scripts.Project.Services.Balance
{
    public interface IProjectBalanceStorage : IProjectBalanceStorageAsyncBase
    {
        ICreatureBalanceStorage Creatures { get; }
        IAbilityBalanceStorage Abilities { get; }
        IInteractrableObjectsBalanceStorage InteractableObjects { get; }
    }

    public class ProjectBalanceStorage : ProjectBalanceStorageAsyncBase, IProjectBalanceStorage
    {
        public ICreatureBalanceStorage Creatures { get; private set; }
        public IAbilityBalanceStorage Abilities { get; private set; }
        public IInteractrableObjectsBalanceStorage InteractableObjects { get; private set; }

        public ProjectBalanceStorage()
        {
            Creatures = new CreatureBalanceStorage();
            Abilities = new AbilityBalanceStorage();
            InteractableObjects = new InteractrableObjectsBalanceStorage();
        }

        protected override HashSet<IProjectSerivce> GetStoragesToInit()
        {
            var result = new HashSet<IProjectSerivce>();
            result.Add(Creatures);
            result.Add(Abilities);
            result.Add(InteractableObjects);
            return result;
        }
    }
}
