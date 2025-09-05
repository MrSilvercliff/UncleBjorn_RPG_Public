using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Popups;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Popups
{
    public interface IPopupHandler
    {
        void OnPreOpen(IPopupWindow view);
        void OnPostOpen(IPopupWindow view);
        void OnPreClose(IPopupWindow view);
        void OnPostClose(IPopupWindow view);
    }
}


