using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.Async;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.Sync;
using IInitializable = ZerglingUnityPlugins.Tools.Scripts.Interfaces.Sync.IInitializable;
using IFlushable = ZerglingUnityPlugins.Tools.Scripts.Interfaces.Async.IFlushable;

namespace ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.SyncAsync
{
    public interface IProjectService : IInitializable, IFlushable
    { 
    }
}