using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Popups;
using ZerglingUnityPlugins.ZenjectExtentions.ContextProvider;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Popups
{
    public class PopupFactory : IFactory<PopupWindow, PopupWindow>
    {
        [Inject] private IZenjectContextProvider _contextProvider;

        public PopupWindow Create(PopupWindow param)
        {
            var container = _contextProvider.Container;
            var result = container.InstantiatePrefabForComponent<PopupWindow>(param);
            return result;
        }
    }
}