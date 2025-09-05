using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Project.ObjectPools
{
    [Serializable]
    public class ObjectPoolItem
    {
        public GameObject Prefab => _prefab;
        public Transform Container => _container;

        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _container;
    }
}