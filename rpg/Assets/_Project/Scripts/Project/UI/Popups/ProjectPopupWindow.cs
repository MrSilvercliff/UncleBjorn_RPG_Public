using System.Threading.Tasks;
using _Project.Scripts.Project.UI.Data;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Log;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Popups;

namespace _Project.Scripts.Project.UI.Popups
{
    public class ProjectPopupWindow : PopupWindow
    {
        [SerializeField] protected UIData _windowUIData;

        protected override Task<bool> OnInit()
        {
            LogUtils.Info(this, "OnInit");
            
            if (_windowUIData != null)
                _windowUIData.Init();
            
            return Task.FromResult(true);
        }
    }
}
