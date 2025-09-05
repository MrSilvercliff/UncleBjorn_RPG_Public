using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ZerglingUnityPlugins.Tools.Scripts.Interfaces.Async
{
    public interface IAwakable
    {
        Task<bool> OnAwake();
    }
}