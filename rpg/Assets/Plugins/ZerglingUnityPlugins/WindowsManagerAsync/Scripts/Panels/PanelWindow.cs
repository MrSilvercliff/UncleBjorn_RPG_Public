using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Panels
{
    public interface IPanelWindow : IWindow
    { 
    }

    public class PanelWindow : Window, IPanelWindow
    {
        [SerializeField] protected Canvas _canvas;

        protected override Task<bool> OnSetup(IWindowSetup setup)
        {
            var panelSetup = (IPanelSettingsConfig)setup;
            _canvas.sortingOrder = panelSetup.CanvasSortOrder;
            return Task.FromResult(true);
        }
    }
}