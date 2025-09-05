using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Popups.Queue
{
    public interface IPopupQueueItem
    {
        Type PopupType { get; }
        IWindowSetup PopupSetup { get; }
    }

    public class PopupQueueItem : IPopupQueueItem
    {
        public Type PopupType { get; }
        public IWindowSetup PopupSetup { get; }

        public PopupQueueItem(Type popupType, IWindowSetup popupSetup)
        {
            PopupType = popupType;
            PopupSetup = popupSetup;
        }
    }
}


