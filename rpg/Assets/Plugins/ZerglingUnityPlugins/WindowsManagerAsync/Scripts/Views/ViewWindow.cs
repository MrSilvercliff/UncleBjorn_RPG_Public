using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Views
{
    public interface IViewWindow : IWindow
    {
    }

    public abstract class ViewWindow : Window, IViewWindow
    {
        protected async override Task<bool> OnInit()
        {
            await base.OnInit();
            gameObject.SetActive(false);
            return true;
        }
    }
}


