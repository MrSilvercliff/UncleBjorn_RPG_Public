using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Project.Monobeh
{
    public interface IProjectMonoBehaviour
    {
        Transform Transform { get; }
        bool ActiveInHierarchy { get; }
        int InstanceID { get; }

        void SetActive(bool active);
    }

    public class ProjectMonoBehaviour : MonoBehaviour, IProjectMonoBehaviour
    {
        public Transform Transform => _transform;
        public bool ActiveInHierarchy => gameObject.activeInHierarchy;
        public int InstanceID => _instanceId;

        [Header("PROJECT MONO BEHAVIOUR")]
        [SerializeField] private Transform _transform;

        private int _instanceId;

        private void Awake()
        {
            _instanceId = gameObject.GetInstanceID();
            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}