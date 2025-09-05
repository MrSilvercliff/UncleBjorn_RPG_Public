using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Panels;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Panels
{
    public class PanelInvokeComponent : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private PanelWindow _panel;
        [SerializeField] private PanelSettingsConfig _config;

        [Inject] private IPanelController _panelController;

        private void OnEnable()
        {
            var panelType = _panel.GetType();
            _config.CanvasSortOrder = _canvas.sortingOrder + 1;
            _panelController.OpenPanel(panelType, _config);
        }

        private void OnDisable()
        {
            var panelType = _panel.GetType();
            _panelController.ClosePanel(panelType);
        }

        public void Flush()
        {
            _config.CanvasSortOrder = 0;
        }
    }
}