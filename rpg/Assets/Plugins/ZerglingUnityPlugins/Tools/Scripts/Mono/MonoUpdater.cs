using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Mono;

namespace Assets.Plugins.ZerglingUnityPlugins.Tools.Scripts.Mono
{
    public interface IMonoUpdater
    {
        void Subscribe(IMonoUpdatable updatable);
        void UnSubscribe(IMonoUpdatable updatable);

        void Subscribe(IMonoFixedUpdatable updatable);
        void UnSubscribe(IMonoFixedUpdatable updatable);

        void Subscribe(IMonoLateUpdatable updatable);
        void UnSubscribe(IMonoLateUpdatable updatable);
    }

    public class MonoUpdater : MonoBehaviour, IMonoUpdater
    {
        private HashSet<IMonoUpdatable> _monoUpdatableObjects;
        private HashSet<IMonoFixedUpdatable> _monoFixedUpdatableObjects;
        private HashSet<IMonoLateUpdatable> _monoLateUpdatableObjects;

        private void Awake()
        {
            _monoUpdatableObjects = new HashSet<IMonoUpdatable>();
            _monoFixedUpdatableObjects = new HashSet<IMonoFixedUpdatable>();
            _monoLateUpdatableObjects = new HashSet<IMonoLateUpdatable>();
        }

        private void Update()
        {
            foreach (var updatable in _monoUpdatableObjects)
                updatable.OnUpdate();
        }

        private void FixedUpdate()
        {
            foreach (var updatable in _monoFixedUpdatableObjects)
                updatable.OnFixedUpdate();
        }

        private void LateUpdate()
        {
            foreach (var updatable in _monoLateUpdatableObjects)
                updatable.OnLateUpdate();
        }

        public void Subscribe(IMonoUpdatable updatable)
        {
            _monoUpdatableObjects.Add(updatable);
        }

        public void UnSubscribe(IMonoUpdatable updatable)
        {
            _monoUpdatableObjects.Remove(updatable);
        }


        public void Subscribe(IMonoFixedUpdatable updatable)
        {
            _monoFixedUpdatableObjects.Add(updatable);
        }

        public void UnSubscribe(IMonoFixedUpdatable updatable)
        {
            _monoFixedUpdatableObjects.Remove(updatable);
        }


        public void Subscribe(IMonoLateUpdatable updatable)
        {
            _monoLateUpdatableObjects.Add(updatable);
        }

        public void UnSubscribe(IMonoLateUpdatable updatable)
        {
            _monoLateUpdatableObjects.Remove(updatable);
        }
    }
}