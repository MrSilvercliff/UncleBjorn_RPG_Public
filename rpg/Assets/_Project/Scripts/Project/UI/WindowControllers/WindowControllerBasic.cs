using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics;

namespace _Project.Scripts.Project.UI.WindowControllers
{
    public class WindowControllerBasic : WindowController
    {
        protected async override Task AnimateOpen(Action completeCallback)
        {
            gameObject.SetActive(true);
            completeCallback?.Invoke();
        }

        protected async override Task AnimateClose(Action completeCallback)
        {
            gameObject.SetActive(false);
            completeCallback?.Invoke();
        }
    }
}