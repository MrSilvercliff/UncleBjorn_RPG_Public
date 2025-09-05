using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Project.Scripts.Project.Enums;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace _Project.Scripts.Project.Scenes
{
    public interface ISceneServiceIniter : IProjectSerivce
    {
        Task<bool> InitServices(int stage);
    }

    public abstract class SceneServiceIniter : ISceneServiceIniter
    {
        private HashSet<ProjectServiceItem> _allServices;
        private HashSet<ProjectServiceItem> _initedServices;

        public SceneServiceIniter()
        {
            _allServices = new();
            _initedServices = new();
        }

        public async Task<bool> Init()
        {
            var result = await OnInit();
            return result;
        }
        
        protected abstract Task<bool> OnInit();

        public bool Flush()
        {
            var classType = GetType().Name;
            Debug.LogError($"[{classType}] Flush START");
            
            foreach (var projectServiceItem in _initedServices)
            {
                var serviceType = projectServiceItem.ServiceType;
                
                if (serviceType == ProjectServiceType.NonFlushable)
                    continue;

                var projectService = projectServiceItem.ProjectService;
                FlushService(projectService);
            }

            _initedServices.Clear();
            _allServices.Clear();
            Debug.LogError($"[{classType}] Flush FINISHED");
            return true;
        }

        private bool FlushService(IProjectSerivce service)
        {
            var serviceName = service.GetType().Name;
            
            LogUtils.Info(this, $"[{serviceName}] flush START");
            
            try
            {
                var result = service.Flush();

                if (!result)
                    return false;
            }
            catch (Exception e)
            {
                LogUtils.Error(this, $"[{serviceName}] flush ERROR");
                LogUtils.Error(this, e.Message);
                LogUtils.Error(this, e.StackTrace);
                return false;
            }

            LogUtils.Info(this, $"[{serviceName}] flush FINISH");
            return true;
        }

        protected void AddService_Flushable(IProjectSerivce service)
        {
            AddService(ProjectServiceType.Flushable, service);
        }

        protected void AddService_NonFlushable(IProjectSerivce service)
        {
            AddService(ProjectServiceType.NonFlushable, service);
        }

        private void AddService(ProjectServiceType serviceType, IProjectSerivce service)
        {
            var projectServiceItem = new ProjectServiceItem(serviceType, service);

            if (_allServices.FirstOrDefault(x => x.ProjectService == service) != null)
                return;
            
            _allServices.Add(projectServiceItem);
        }

        protected async Task<bool> InitServices()
        {
            foreach (var projectServiceItem in _allServices)
            {
                if (_initedServices.Contains(projectServiceItem))
                    continue;

                var projectService = projectServiceItem.ProjectService;
                var initResult = await InitService(projectService);

                if (!initResult)
                    return false;

                _initedServices.Add(projectServiceItem);
            }

            return true;
        }

        private async Task<bool> InitService(IProjectSerivce service)
        {
            var serviceName = service.GetType().Name;

            LogUtils.Info(this, $"[{serviceName}] init START");

            try
            {
                var initResult = await service.Init();

                if (!initResult)
                    return false;
            }
            catch (Exception e)
            {
                LogUtils.Error(this, $"[{serviceName}] init ERROR");
                LogUtils.Error(this, e.Message);
                LogUtils.Error(this, e.StackTrace);
                return false;
            }

            LogUtils.Info(this, $"[{serviceName}] init SUCCESS");
            return true;
        }

        public async Task<bool> InitServices(int stage)
        {
            var classType = GetType().Name;
            Debug.LogError($"[{classType}] InitServices [{stage}] START");
            var result = await OnInitServices(stage);
            Debug.LogError($"[{classType}] InitServices [{stage}] FINISHED");
            return result;
        }

        protected abstract Task<bool> OnInitServices(int stage);

        private class ProjectServiceItem
        {
            public ProjectServiceType ServiceType { get; }
            public IProjectSerivce ProjectService { get; }
            
            public ProjectServiceItem(ProjectServiceType serviceType, IProjectSerivce projectService)
            {
                ServiceType = serviceType;
                ProjectService = projectService;
            }
        }
    }
}