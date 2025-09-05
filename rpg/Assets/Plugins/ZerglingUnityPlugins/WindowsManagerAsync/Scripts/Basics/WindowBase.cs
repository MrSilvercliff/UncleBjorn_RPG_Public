using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.Async;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.Sync;
using ZerglingUnityPlugins.Tools.Scripts.Log;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Panels;
using IFlushable = ZerglingUnityPlugins.Tools.Scripts.Interfaces.Sync.IFlushable;
using IInitializable = ZerglingUnityPlugins.Tools.Scripts.Interfaces.Async.IInitializable;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics
{
    public interface IWindowBase : IInitializable, IFlushable
    {
        IWindowController Controller { get; }
    }

    public abstract class WindowBase : MonoBehaviour, IWindowBase
    {
        public IWindowController Controller { get; private set; }

        public async Task<bool> Init()
        {
            gameObject.SetActive(false);
            Controller = GetComponent<WindowController>();
            
            if (Controller == null)
                LogUtils.Error(gameObject.name, $"WINDOW CONTROLLER IS NULL");
            else
                await Controller.Init();
            
            var result = await OnInit();
            return result;
        }

        protected virtual Task<bool> OnInit()
        {
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            Controller.Flush();
            Controller = null;
            TryFlushPanelInvokeComponent();
            var result = OnFlush();
            return result;
        }

        protected virtual bool OnFlush()
        {
            return true;
        }

        private void TryFlushPanelInvokeComponent()
        {
            var getComponentResult = TryGetComponent<PanelInvokeComponent>(out var panelInvokeComponent);

            if (!getComponentResult)
                return;

            panelInvokeComponent.Flush();
        }
    }
}


