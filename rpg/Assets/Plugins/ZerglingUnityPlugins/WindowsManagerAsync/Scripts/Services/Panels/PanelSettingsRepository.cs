using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Panels;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Panels
{
    public interface IPanelSettingsRepository : IProjectSerivce
    {
        void Add(Type panelType, IPanelSettingsConfig config);
        IPanelSettingsConfig Pop(Type panelType);
        IPanelSettingsConfig Peek(Type panelType);
        int Count(Type panelType);
    }

    public class PanelSettingsRepository : IPanelSettingsRepository
    {
        private Dictionary<Type, Stack<IPanelSettingsConfig>> _byPanelType;

        public Task<bool> Init()
        {
            _byPanelType = new Dictionary<Type, Stack<IPanelSettingsConfig>>();
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            return true;
        }

        public void Add(Type panelType, IPanelSettingsConfig config)
        {
            if (!_byPanelType.ContainsKey(panelType))
                _byPanelType[panelType] = new Stack<IPanelSettingsConfig>();

            var stack = _byPanelType[panelType];
            stack.Push(config);
        }

        public IPanelSettingsConfig Pop(Type panelType)
        {
            if (!_byPanelType.ContainsKey(panelType))
                return null;

            var stack = _byPanelType[panelType];
            var result = stack.Pop();
            return result;
        }

        public IPanelSettingsConfig Peek(Type panelType)
        {
            if (!_byPanelType.ContainsKey(panelType))
                return null;

            var stack = _byPanelType[panelType];
            var result = stack.Peek();
            return result;
        }

        public int Count(Type panelType)
        {
            if (!_byPanelType.ContainsKey(panelType))
                return 0;

            var stack = _byPanelType[panelType];
            return stack.Count;
        }
    }
}