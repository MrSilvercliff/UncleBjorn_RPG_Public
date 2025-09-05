using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.Async;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.Sync;
using IInitializable = ZerglingUnityPlugins.Tools.Scripts.Interfaces.Async.IInitializable;
using IFlushable = ZerglingUnityPlugins.Tools.Scripts.Interfaces.Sync.IFlushable;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics
{
    public interface IWindowController : IInitializable, IFlushable
    {
        Canvas Canvas { get; }

        Action OnOpenEvent { get; set; }
        Action OnCloseEvent { get; set; }

        Task Open();
        Task Close();
    }

    public class WindowController : MonoBehaviour, IWindowController
    {
        public Canvas Canvas { get; private set; }
        public Action OnOpenEvent { get; set; }
        public Action OnCloseEvent { get; set; }

        public async Task<bool> Init()
        {
            Canvas = GetComponent<Canvas>();
            var result =  await OnInit();
            return result;
        }

        protected virtual Task<bool> OnInit()
        {
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            Canvas = null;
            return true;
        }

        public async Task Open()
        {
            await AnimateOpen(OnOpened);
        }

        protected virtual Task AnimateOpen(Action completeCallback)
        {
            completeCallback?.Invoke();
            return Task.CompletedTask;
        }

        private void OnOpened()
        {
            OnOpenEvent?.Invoke();
        }

        public async Task Close()
        {
            await AnimateClose(OnClosed);
        }

        protected virtual Task AnimateClose(Action completeCallback)
        {
            completeCallback?.Invoke();
            return Task.CompletedTask;
        }

        private void OnClosed()
        {
            OnCloseEvent?.Invoke();
        }
    }
}


