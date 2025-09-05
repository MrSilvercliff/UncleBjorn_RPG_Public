using _Project.Scripts.Project.Services.SceneLoading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace _Project.Scripts.Project.Scenes
{
    public interface ISceneController
    {
        Task LateStart();
        void Flush();
    }

    public abstract class SceneController : MonoBehaviour, ISceneController
    {
        [Inject] private ISceneControllerProvider _sceneControllerProvider;
        [Inject] private ISceneLoadController _sceneLoadController;

        private async void Awake()
        {
            LogUtils.Info(this, "AWAKE START");

            try
            {
                await OnAwake();
            }
            catch (Exception e)
            {
                LogUtils.Error(this, "AWAKE ERROR");
                LogUtils.Error(this, e.Message);
                LogUtils.Error(this, e.StackTrace);
                throw;
            }

            LogUtils.Info(this, "AWAKE FINISH");
        }

        private async void Start()
        {
            LogUtils.Info(this, "START START");

            try
            {
                await OnStart();
            }
            catch (Exception e)
            {
                LogUtils.Error(this, "START ERROR");
                LogUtils.Error(this, e.Message);
                LogUtils.Error(this, e.StackTrace);
                throw;
            }

            LogUtils.Info(this, "START FINISH");

            _sceneControllerProvider.SetCurrentSceneController(this);
            _sceneLoadController.OnSceneStartFinished();
        }

        public async Task LateStart()
        {
            LogUtils.Info(this, "LATE START START");

            try
            {
                await OnLateStart();
            }
            catch (Exception e)
            {
                LogUtils.Error(this, "LATE START ERROR");
                LogUtils.Error(this, e.Message);
                LogUtils.Error(this, e.StackTrace);
                throw;
            }

            LogUtils.Info(this, "LATE START FINISH");
        }

        public void Flush()
        {
            LogUtils.Info(this, "FLUSH START");

            OnFlush();

            LogUtils.Info(this, "FLUSH FINISH");
        }

        protected abstract Task OnAwake();

        protected abstract Task OnStart();

        protected abstract Task OnLateStart();

        protected abstract void OnFlush();
    }
}