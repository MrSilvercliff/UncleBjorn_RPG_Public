using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Project.ObjectPools
{
    public interface IProjectObjectPoolContainers
    {
        ObjectPoolItem AudioSourceController { get; }
    }

    public class ProjectObjectPoolContainers : MonoBehaviour, IProjectObjectPoolContainers
    {
        public ObjectPoolItem AudioSourceController => _audioSourceController;

        [SerializeField] private ObjectPoolItem _audioSourceController;
    }
}