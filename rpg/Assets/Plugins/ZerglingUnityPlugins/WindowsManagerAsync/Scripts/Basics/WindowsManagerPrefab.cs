using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Popups;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Views;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics
{
    public interface IWindowsManagerPrefab
    { 
        IViewController ViewController { get; }
        IPopupController PopupController { get; }
    }

    public class WindowsManagerPrefab : MonoBehaviour, IWindowsManagerPrefab
    {
        public IViewController ViewController => _viewController;
        public IPopupController PopupController => _popupController;

        [SerializeField] private ViewControllerBase _viewController;
        [SerializeField] private PopupControllerBase _popupController;
    }
}