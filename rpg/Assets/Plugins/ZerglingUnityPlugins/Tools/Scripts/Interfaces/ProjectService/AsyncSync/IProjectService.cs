using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.Async;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.Sync;
using IInitializable = ZerglingUnityPlugins.Tools.Scripts.Interfaces.Async.IInitializable;
using IFlushable = ZerglingUnityPlugins.Tools.Scripts.Interfaces.Sync.IFlushable;

namespace ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync
{
    public interface IProjectSerivce : IInitializable, IFlushable
    { 
    }
}