using ZerglingUnityPlugins.Tools.Scripts.Log;
using UnityEngine;

namespace ZerglingUnityPlugins.Tools.Scripts.Configs
{
    public interface IConfigBase
    {
        void Init();
    }

    /// <summary>
    /// не забудь в дочернем классе добавить
    /// [CreateAssetMenu(fileName = "T.Name", menuName = "Configs/T.Name")]
    /// чтобы добавить пункт меню для создания конфига
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfigBase<T> : ScriptableObject, IConfigBase where T : Object
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Resources.Load<T>(typeof(T).Name);

                return _instance;
            }
        }

        public virtual void Init()
        {
            LogUtils.Info(typeof(T).Name, $"Init");
        }
    }
}
