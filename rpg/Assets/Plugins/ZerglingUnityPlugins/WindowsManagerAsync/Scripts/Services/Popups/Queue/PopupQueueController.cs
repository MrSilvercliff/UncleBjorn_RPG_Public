using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Popups;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Popups.Queue
{
    public interface IPopupQueueController : IProjectSerivce
    {
        void Enqueue<T>(IWindowSetup setup) where T : IPopupWindow;
        void Enqueue(Type popupType, IWindowSetup setup);
        Task<bool> TryOpenNextPopup();

        void Clear();
    }

    public class PopupQueueController : IPopupQueueController
    {
        [Inject] private IPopupController _popupController;
        [Inject] private IPopupRepository _popupRepository;

        private Queue<IPopupQueueItem> _queue;

        public Task<bool> Init()
        {
            _queue = new Queue<IPopupQueueItem>();
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            return true;
        }

        public void Enqueue<T>(IWindowSetup setup) where T : IPopupWindow
        {
            var popupType = typeof(T);
            Enqueue(popupType, setup);
        }

        public void Enqueue(Type popupType, IWindowSetup setup)
        {
            if (_popupRepository.Count == 0)
            {
                _popupController.OpenPopup(popupType, setup);
                return;
            }

            AddToQueue(popupType, setup);
        }

        public async Task<bool> TryOpenNextPopup()
        {
            if (_queue.Count == 0)
                return false;

            var popupItem = _queue.Dequeue();
            var popupType = popupItem.PopupType;
            var popupSetup = popupItem.PopupSetup;

            await _popupController.OpenPopup(popupType, popupSetup);
            return true;
        }

        public void Clear()
        {
            _queue.Clear();
        }

        private void AddToQueue(Type popupType, IWindowSetup setup)
        {
            var queueItem = new PopupQueueItem(popupType, setup);
            _queue.Enqueue(queueItem);
        }
    }
}


