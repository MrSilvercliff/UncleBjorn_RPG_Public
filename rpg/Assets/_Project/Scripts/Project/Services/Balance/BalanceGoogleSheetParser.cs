using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.GoogleSheetParse;
using UnityEngine;

namespace _Project.Scripts.Project.Services.Balance
{
    public class BalanceGoogleSheetParser : BalanceGoogleSheetParserBase
    {
        protected override void FillSheetsDictionary()
        {
            var sheets = _config.GoogleSheetPages;

            var startProfileSheet = sheets[0].PageName;
            _sheets[startProfileSheet] = ParseAsIsConfig;

            var creaturesSheet = sheets[1].PageName;
            _sheets[creaturesSheet] = ParseAsIsDictionary;

            var abilitiesSheet = sheets[2].PageName;
            _sheets[abilitiesSheet] = ParseAsIsDictionary;

            var interactableObjectsSheet = sheets[3].PageName;
            _sheets[interactableObjectsSheet] = ParseAsIsConfig;
        }
    }
}
