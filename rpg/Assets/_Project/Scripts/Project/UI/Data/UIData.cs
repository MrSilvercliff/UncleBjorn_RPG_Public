using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace _Project.Scripts.Project.UI.Data
{
    public interface IUIData
    {
        void Init();
    }

    public abstract class UIData : MonoBehaviour, IUIData
    {
        public virtual void Init()
        {
            LogUtils.Info(this, "Init");
        }
    }
}
