using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;
using ZerglingUnityPlugins.Tools.Scripts.Log;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Configs;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Popups;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Popups
{
    public interface IPopupController : IProjectSerivce
    {
        Task<IPopupWindow> OpenPopup<TPopup>() where TPopup : IPopupWindow;
        Task<IPopupWindow> OpenPopup<TPopup>(IWindowSetup setup) where TPopup : IPopupWindow;
        Task<IPopupWindow> OpenPopup(Type popupType);
        Task<IPopupWindow> OpenPopup(Type popupType, IWindowSetup setup);

        Task ClosePopup(IPopupWindow popup);
        Task CloseAll();
    }

    public abstract class PopupControllerBase : MonoBehaviour, IPopupController
    {
        [Inject] protected IPopupsConfig _popupsConfig;
        [Inject] protected IPopupHandler _popupHandler;

        public async Task<bool> Init()
        {
            var result = await OnInit();
            return result;
        }

        protected virtual Task<bool> OnInit()
        {
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            var result = OnFlush();
            return result;
        }

        protected virtual bool OnFlush()
        {
            return true;
        }

        protected virtual IPopupWindow GetPopup(Type popupType)
        {
            return null;
        }

        public async Task<IPopupWindow> OpenPopup<TPopup>() where TPopup : IPopupWindow
        {
            var popupType = typeof(TPopup);
            var popup = await OpenPopup(popupType, WindowSetupEmpty.Instance);
            return popup;
        }

        public async Task<IPopupWindow> OpenPopup<TPopup>(IWindowSetup setup) where TPopup : IPopupWindow
        {
            var popupType = typeof(TPopup);
            var popup = await OpenPopup(popupType, setup);
            return popup;
        }

        public async Task<IPopupWindow> OpenPopup(Type popupType)
        {
            var popup = await OpenPopup(popupType, WindowSetupEmpty.Instance);
            return popup;
        }

        public async Task<IPopupWindow> OpenPopup(Type popupType, IWindowSetup setup)
        {
            var popup = GetPopup(popupType);

            try
            {
                await popup.Setup(setup);

                await popup.PreOpen();
                _popupHandler.OnPreOpen(popup);

                await popup.Controller.Open();
                await OnPopupOpened(popup);

                await popup.PostOpen();
                _popupHandler.OnPostOpen(popup);
            }
            catch (Exception ex)
            {
                LogUtils.Error(this, $"[{popupType}] OPEN ERROR");
                LogUtils.Error(this, ex.Message);
                LogUtils.Error(this, ex.StackTrace);
                return null;
            }

            return popup;
        }
        
        protected abstract Task OnPopupOpened(IPopupWindow popup);

        public async Task ClosePopup(IPopupWindow popup)
        {
            try
            {
                await popup.PreClose();
                _popupHandler.OnPreClose(popup);

                await popup.Controller.Close();
                await OnPopupClosed(popup);

                await popup.PostClose();
                _popupHandler.OnPostClose(popup);
            }
            catch (Exception ex)
            {
                var popupType = popup.GetType();
                LogUtils.Error(this, $"[{popupType}] CLOSE ERROR");
                LogUtils.Error(this, ex.Message);
                LogUtils.Error(this, ex.StackTrace);
            }
        }
        
        protected abstract Task OnPopupClosed(IPopupWindow popup);

        public async Task CloseAll()
        {
            await OnCloseAll();
        }

        protected virtual Task OnCloseAll()
        {
            return Task.CompletedTask;
        }
    }
}


