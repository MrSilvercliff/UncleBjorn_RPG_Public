using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Views;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Views
{
    public interface IViewHandler
    {
        void OnPreOpen(IViewWindow view);
        void OnPostOpen(IViewWindow view);
        void OnPreClose(IViewWindow view);
        void OnPostClose(IViewWindow view);
    }
}

