using _Project.Scripts.Project.Services.Balance;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.Configs;
using UnityEngine;

namespace _Project.Scripts.Project.Configs
{
    [CreateAssetMenu(fileName = "ProjectBalanceConfig", menuName = "Project/Configs/Project/Project Balance Config")]
    public class ProjectBalanceConfig : BalanceConfigBase
    {
        public override void ParseBalance()
        {
            var parser = new BalanceGoogleSheetParser();
            parser.Setup(this);
            parser.ParseBalance();
        }
    }
}
