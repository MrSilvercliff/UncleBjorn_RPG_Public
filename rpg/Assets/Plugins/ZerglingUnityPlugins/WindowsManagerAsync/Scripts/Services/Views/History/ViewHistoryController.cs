using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Views;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Views.History
{
    public interface IViewHistoryController : IProjectSerivce
    {
        void OnViewOpened(ViewWindow viewWindow);
        Task OpenPreviousView();
    }

    public class ViewHistoryService : IViewHistoryController
    {
        [Inject] private IViewController _viewController;

        private Stack<IViewWindow> _history;

        public Task<bool> Init()
        {
            _history = new Stack<IViewWindow>();
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            _history.Clear();
            return true;
        }

        public void OnViewOpened(ViewWindow viewWindow)
        {
            _history.Push(viewWindow);
        }

        public async Task OpenPreviousView()
        {
            _history.Pop();
            var previousView = _history.Pop();
            var previousViewType = previousView.GetType();
            await _viewController.OpenView(previousViewType, WindowSetupEmpty.Instance);
        }
    }
}


