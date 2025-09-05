using _Project.Scripts.Project.Enums;
using _Project.Scripts.Project.Handlers.SceneLoading;
using _Project.Scripts.Project.UI.Popups.SceneLoading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Log;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Popups;
using ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Services.Popups;

namespace _Project.Scripts.Project.Services.SceneLoading
{
    public interface ISceneLoadController
    {
        SceneName CurrentSceneName { get; }

        void LoadScene(string sceneName);
        void OnSceneStartFinished();
    }

    public class SceneLoadController : MonoBehaviour, ISceneLoadController
    {
        public SceneName CurrentSceneName => _currentSceneName;

        [Inject] private IPopupController _popupController;
        [Inject] private ISceneControllerProvider _sceneControllerProvider;

        [Inject] private ISceneLoadHandler _handler;

        private Scene? _currentScene;
        private Scene? _previousScene;

        private SceneName _currentSceneName;

        private Coroutine _loadSceneCoroutine;
        private IPopupWindow _sceneLoadingPopup;
        private bool _sceneStartFinished;

        private void Awake()
        {
            LogUtils.Info(this, $"Awake");

            _currentScene = null;
            _previousScene = null;

            _loadSceneCoroutine = null;
            _sceneLoadingPopup = null;
        }

        private void OnEnable()
        {
            LogUtils.Info(this, $"OnEnable");

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            LogUtils.Info(this, $"OnDisable");

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void LoadScene(string sceneName)
        {
            _previousScene = _currentScene;

            _sceneStartFinished = false;

            KillCoroutine();

            _loadSceneCoroutine = StartCoroutine(LoadSceneTask(sceneName));
        }

        public void OnSceneStartFinished()
        {
            LogUtils.Info(this, $"{_currentSceneName} start finished");
            _sceneStartFinished = true;
        }

        private IEnumerator LoadSceneTask(string sceneName)
        {
            yield return OpenSceneLoadingPopup();

            yield return FlushCurrenctScene();

            yield return OnSceneLoadingProgress(1.0f / 3.0f);

            yield return LoadSceneInternal(sceneName);

            yield return OnSceneLoadingProgress(2.0f / 3.0f);

            yield return WaitSceneStartFinish();

            yield return OnSceneLoadingProgress(3.0f / 3.0f);

            yield return CloseSceneLoadingPopup();

            yield return CurrentSceneLateStart();
        }

        private IEnumerator OpenSceneLoadingPopup()
        {
            var openTask = _popupController.OpenPopup<SceneLoadingPopup>();

            while (!openTask.IsCompleted)
                yield return null;

            _sceneLoadingPopup = openTask.Result;
        }

        private IEnumerator FlushCurrenctScene()
        {
            var currentSceneController = _sceneControllerProvider.CurrentSceneController;

            currentSceneController.Flush();

            yield return null;
        }

        private IEnumerator LoadSceneInternal(string sceneName)
        {
            var loadSceneTask = SceneManager.LoadSceneAsync(sceneName);
            loadSceneTask.allowSceneActivation = false;

            while (!loadSceneTask.isDone)
            {
                if (loadSceneTask.progress >= 0.9f)
                    break;

                yield return null;
            }

            loadSceneTask.allowSceneActivation = true;
        }

        private IEnumerator WaitSceneStartFinish()
        {
            while (!_sceneStartFinished)
                yield return null;
        }

        private IEnumerator CloseSceneLoadingPopup()
        {
            var closeTask = _popupController.ClosePopup(_sceneLoadingPopup);

            while (!closeTask.IsCompleted)
                yield return null;

            _sceneLoadingPopup = null;
        }

        private IEnumerator CurrentSceneLateStart()
        {
            var currentSceneController = _sceneControllerProvider.CurrentSceneController;

            var lateStartTask = currentSceneController.LateStart();

            while (!lateStartTask.IsCompleted)
                yield return null;
        }

        private IEnumerator OnSceneLoadingProgress(float progress)
        {
            _handler.OnSceneLoadingProgress(progress);

            yield return new WaitForSeconds(1.5f);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            LogUtils.Info(this, $"{scene.name} loaded via {loadSceneMode} mode");

            _currentScene = scene;

            var nameParseResult = Enum.TryParse<SceneName>(_currentScene.Value.name, out var sceneName);
            if (!nameParseResult)
            {
                LogUtils.Error(this, $"scene name parse error!");
                return;
            }

            _currentSceneName = sceneName;
        }

        private void KillCoroutine()
        {
            if (_loadSceneCoroutine == null)
                return;

            StopCoroutine(_loadSceneCoroutine);
            _loadSceneCoroutine = null;
        }
    }
}