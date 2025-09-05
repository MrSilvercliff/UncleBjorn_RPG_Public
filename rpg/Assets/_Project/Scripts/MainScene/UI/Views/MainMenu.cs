using System.Threading.Tasks;
using Zenject;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics;
using _Project.Scripts.Project.Enums;
using _Project.Scripts.Project.Services.SceneLoading;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Project.UI.Views.MainMenu
{
    public class MainMenu : ProjectViewWindow
    {
        [SerializeField] private Button _buttonGoToGameScene;
        [SerializeField] private Button _buttonQuit;

        [Inject] private ISceneLoadController _sceneLoadController;

        protected override Task OnPreOpen()
        {
            SubscribeUIElements();
            return Task.FromResult(true);
        }

        private void SubscribeUIElements()
        { 
            _buttonGoToGameScene.onClick.AddListener(OnStartGameClick);
            _buttonQuit.onClick.AddListener(OnQuitGameClick);
        }

        protected override Task OnPreClose()
        {
            UnSubscribeUIElements();
            return Task.FromResult(true);
        }

        private void UnSubscribeUIElements()
        {
            _buttonGoToGameScene.onClick.RemoveListener(OnStartGameClick);
            _buttonQuit.onClick.RemoveListener(OnQuitGameClick);
        }

        protected override Task<bool> OnSetup(IWindowSetup setup)
        {
            return Task.FromResult(true);
        }

        private void OnStartGameClick()
        {
            var sceneName = SceneName.GameScene.ToString();
            _sceneLoadController.LoadScene(sceneName);
        }

        private void OnQuitGameClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();  
#endif
        }
    }
}