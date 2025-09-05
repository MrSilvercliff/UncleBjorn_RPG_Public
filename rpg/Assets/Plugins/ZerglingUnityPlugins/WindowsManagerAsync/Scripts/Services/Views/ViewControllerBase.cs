using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;
using ZerglingUnityPlugins.Tools.Scripts.Log;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Configs;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Views;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Views
{
    public interface IViewController : IProjectSerivce
    { 
        IViewWindow ActiveView { get; }
        Task OpenView<TView>() where TView : IViewWindow;
        Task OpenView<TView>(IWindowSetup setup) where TView : IViewWindow;
        Task OpenView(Type viewType);
        Task OpenView(Type viewType, IWindowSetup setup);
    }

    public abstract class ViewControllerBase : MonoBehaviour, IViewController
    {
        public IViewWindow ActiveView => _activeView;

        [Inject] protected IViewsConfig _viewsConfig;
        [Inject] protected IViewHandler _viewHandler;

        private IViewWindow _activeView;

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
            _activeView = null;
            var result = OnFlush();
            return result;
        }

        protected virtual bool OnFlush()
        { 
            return true;
        }

        protected virtual IViewWindow GetView(Type viewType)
        {
            return null;
        }

        public async Task OpenView<TWindow>() where TWindow : IViewWindow
        {
            var viewType = typeof(TWindow);
            await OpenView(viewType, WindowSetupEmpty.Instance);
        }

        public async Task OpenView<TWindow>(IWindowSetup setup) where TWindow : IViewWindow
        {
            var viewType = typeof(TWindow);
            await OpenView(viewType, setup);
        }

        public async Task OpenView(Type viewType)
        {
            await OpenView(viewType, WindowSetupEmpty.Instance);
        }

        public async Task OpenView(Type viewType, IWindowSetup setup)
        {
            if (_activeView?.GetType() == viewType)
            {
                LogUtils.Error(this, $"{viewType} is already opened");
                return;
            }

            try 
            {
                if (_activeView != null)
                { 
                    await _activeView.PreClose();
                    _viewHandler.OnPreClose(_activeView);

                    await _activeView.Controller.Close();
                    await OnViewClosed(_activeView);

                    await _activeView.PostClose();
                    _viewHandler.OnPostClose(_activeView);
                }
            }
            catch(Exception ex) 
            {
                var activeViewType = _activeView.GetType().Name;
                LogUtils.Error(this, $"[{activeViewType}] CLOSE ERROR");
                LogUtils.Error(this, ex.Message);
                LogUtils.Error(this, ex.StackTrace);
                return;
            }

            var newView = GetView(viewType);

            if (newView == null)
            {
                LogUtils.Error(this, $"NOT FOUND VIEW {viewType}");
                return;
            }

            try
            {
                await newView.Setup(setup);
                
                await newView.PreOpen();
                _viewHandler.OnPreOpen(newView);

                await newView.Controller.Open();
                await OnViewOpened(newView);

                await newView.PostOpen();
                _viewHandler.OnPostOpen(newView);
            }
            catch (Exception ex)
            {
                LogUtils.Error(this, $"[{viewType}] OPEN ERROR");
                LogUtils.Error(this, ex.Message);
                LogUtils.Error(this, ex.StackTrace);
                return;
            }

            _activeView = newView;
        }

        protected abstract Task OnViewOpened(IViewWindow view);
        
        protected abstract Task OnViewClosed(IViewWindow view);
    }
}

