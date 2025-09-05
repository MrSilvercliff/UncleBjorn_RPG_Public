using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Popups
{
    public interface IPopupWindow : IWindow
    {
    }

    public abstract class PopupWindow : Window, IPopupWindow
    {
        public class Factory : PlaceholderFactory<PopupWindow, PopupWindow> { }
    }
}


