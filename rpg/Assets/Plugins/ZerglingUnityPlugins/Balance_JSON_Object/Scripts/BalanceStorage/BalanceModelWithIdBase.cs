using UnityEngine;

namespace Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.BalanceStorage
{
    public interface IBalanceModelWithIdBase : IBalanceModelBase
    {
        string Id { get; }
    }

    public abstract class BalanceModelWithIdBase : BalanceModelBase, IBalanceModelWithIdBase
    {
        public string Id => _id;

        protected string _id;
    }
}
