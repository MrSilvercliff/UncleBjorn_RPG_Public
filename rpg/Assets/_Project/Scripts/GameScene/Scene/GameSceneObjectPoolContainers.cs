using _Project.Scripts.Project.ObjectPools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameScene.Scene
{
    public interface IGameSceneObjectPoolContainers
    {
        ObjectPoolItem PlayerController { get; }
        ObjectPoolItem CreaturePrefab { get; }
    }

    public class GameSceneObjectPoolContainers : MonoBehaviour, IGameSceneObjectPoolContainers
    {
        public ObjectPoolItem PlayerController => _playerController;
        public ObjectPoolItem CreaturePrefab => _creaturePrefab;

        [SerializeField] private ObjectPoolItem _playerController;
        [SerializeField] private ObjectPoolItem _creaturePrefab;
    }
}