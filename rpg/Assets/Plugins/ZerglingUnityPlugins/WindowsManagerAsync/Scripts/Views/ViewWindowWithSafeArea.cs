using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Views
{
    public interface IViewWindowWithSafeArea : IViewWindow
    {
    }

    public abstract class ViewWindowWithSafeArea : ViewWindow, IViewWindowWithSafeArea
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _contentMin;
        [SerializeField] private RectTransform _contentMax;
        [SerializeField] private RectTransform _contentMinMax;

        protected async override Task<bool> OnInit()
        {
            WindowSafeAreaApplyer.ApplySafeArea(_canvas, _contentMin, _contentMax, _contentMinMax);
            return true;
        }
    }
}


