using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Panels;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Panels
{
    public interface IPanelHandler
    {
        void OnPreOpen(IPanelWindow view);
        void OnPostOpen(IPanelWindow view);
        void OnPreClose(IPanelWindow view);
        void OnPostClose(IPanelWindow view);
    }
}