using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics
{
    public interface IWindow : IWindowBase
    {
        Task<bool> Setup(IWindowSetup setup);
        Task PreOpen();
        Task PostOpen();
        Task PreClose();
        Task PostClose();
    }

    public abstract class Window : WindowBase, IWindow
    {
        public async Task<bool> Setup(IWindowSetup setup)
        {
            var result = await OnSetup(setup);
            return result;
        }

        protected virtual Task<bool> OnSetup(IWindowSetup setup)
        {
            return Task.FromResult(true);
        }

        public async Task PreOpen()
        {
            await OnPreOpen();
        }

        protected virtual Task OnPreOpen()
        {
            return Task.CompletedTask;
        }

        public async Task PostOpen()
        {
            await OnPostOpen();
        }

        protected virtual Task OnPostOpen()
        {
            return Task.CompletedTask;
        }

        public async Task PreClose()
        {
            await OnPreClose();
        }

        protected virtual Task OnPreClose()
        {
            return Task.CompletedTask;
        }

        public async Task PostClose()
        {
            await OnPostClose();
        }

        protected virtual Task OnPostClose()
        {
            return Task.CompletedTask;
        }
    }
}


