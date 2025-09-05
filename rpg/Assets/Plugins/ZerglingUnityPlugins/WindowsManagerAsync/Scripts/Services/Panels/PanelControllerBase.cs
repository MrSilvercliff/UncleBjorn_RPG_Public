using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;
using ZerglingUnityPlugins.Tools.Scripts.Log;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Configs;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Panels;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Popups;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Panels
{
    public interface IPanelController : IProjectSerivce
    {
        Task OpenPanel<TPanel>(IPanelSettingsConfig settingsConfig) where TPanel : IPanelWindow;
        Task OpenPanel(Type panelType, IPanelSettingsConfig settingsConfig);

        Task ClosePanel<TPanel>() where TPanel : IPanelWindow;
        Task ClosePanel(Type panelType);
    }

    public abstract class PanelControllerBase : MonoBehaviour, IPanelController
    {
        [Inject] protected IPanelsConfig _panelsConfig;
        [Inject] protected IPanelSettingsRepository _panelSettingsRepository;
        [Inject] protected IPanelHandler _panelHandler;

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

        protected virtual IPanelWindow GetPanel(Type panelType)
        {
            return null;
        }

        public async Task OpenPanel<TPanel>(IPanelSettingsConfig settingsConfig) where TPanel : IPanelWindow
        {
            var panelType = typeof(TPanel);
            await OpenPanel(panelType, settingsConfig);
        }

        public async Task OpenPanel(Type panelType, IPanelSettingsConfig settingsConfig)
        {
            var panel = GetPanel(panelType);

            if (panel == null)
                return;

            try
            {
                _panelSettingsRepository.Add(panelType, settingsConfig);

                await panel.Setup(settingsConfig);

                await panel.PreOpen();
                _panelHandler.OnPreOpen(panel);

                await panel.Controller.Open();
                await OnPanelOpened(panel);

                await panel.PostOpen();
                _panelHandler.OnPostOpen(panel);
            }
            catch (Exception ex)
            {
                LogUtils.Error(this, $"[{panelType}] OPEN ERROR");
                LogUtils.Error(this, ex.Message);
                LogUtils.Error(this, ex.StackTrace);
            }
        }
        
        protected abstract Task OnPanelOpened(IPanelWindow panel);

        public async Task ClosePanel<TPanel>() where TPanel : IPanelWindow
        {
            var panelType = typeof(TPanel);
            await ClosePanel(panelType);
        }

        public async Task ClosePanel(Type panelType)
        {
            var panel = GetPanel(panelType);

            try
            {
                _panelSettingsRepository.Pop(panelType);

                var settingsCountLeft = _panelSettingsRepository.Count(panelType);

                if (settingsCountLeft > 0)
                {
                    var settingsConfig = _panelSettingsRepository.Peek(panelType);
                    await panel.Setup(settingsConfig);
                }
                else 
                {
                    await panel.PreClose();
                    _panelHandler.OnPreClose(panel);

                    await panel.Controller.Close();
                    await OnPanelClosed(panel);

                    await panel.PostClose();
                    _panelHandler.OnPostClose(panel);
                }
            }
            catch (Exception ex)
            {
                LogUtils.Error(this, $"[{panelType}] CLOSE ERROR");
                LogUtils.Error(this, ex.Message);
                LogUtils.Error(this, ex.StackTrace);
            }
        }
        
        protected abstract Task OnPanelClosed(IPanelWindow panel);
    }
}