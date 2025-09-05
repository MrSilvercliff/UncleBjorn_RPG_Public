namespace ZerglingUnityPlugins.Tools.Scripts.Singleton
{
    public interface ISingleton<T>
    {
        void Init();
    }

    public abstract class Singleton<T> : ISingleton<T> where T : Singleton<T>, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new T();

                return _instance;
            }
        }

        protected Singleton()
        {
            _instance = (T)this;
        }

        public virtual void Init()
        {
        }
    }
}
