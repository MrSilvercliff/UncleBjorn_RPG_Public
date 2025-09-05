using UnityEngine;

namespace ZerglingUnityPlugins.Tools.Scripts.ObjectPool
{
    public class MonoBehaviourObjectPoolSetup<T> : MonoBehaviour where T : MonoBehaviour
    {
        public T Prefab => _prefab;
        public Transform Container => _container;
        public int InitCount => _initCount;
        public int MaxCount => _maxCount > 0 ? _maxCount : int.MaxValue;
        
        [SerializeField] private T _prefab;
        [SerializeField] private Transform _container;
        [SerializeField] private int _initCount;
        [SerializeField] private int _maxCount;
    }
}

