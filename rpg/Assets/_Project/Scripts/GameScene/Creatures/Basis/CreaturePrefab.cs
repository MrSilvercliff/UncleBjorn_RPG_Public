using _Project.Scripts.GameScene.Creatures.Basis.Components;
using _Project.Scripts.Project.Monobeh;
using _Project.Scripts.Project.Zenject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameScene.Creatures.Basis
{
    public interface ICreaturePrefab : IProjectMonoBehaviour, IProjectPoolable
    { 
        string Id { get; }
        IReadOnlyCollection<CreatureComponentBase> Components { get; }
    }

    public class CreaturePrefab : ProjectMonoBehaviour, ICreaturePrefab
    {
        public string Id => _id;
        public IReadOnlyCollection<CreatureComponentBase> Components => _componentsList;

        [Header("CREATURE VIEW")]
        [SerializeField] private string _id;
        [SerializeField] private CreatureComponentBase[] _componentsList;

        public void OnCreated()
        {
        }

        public void OnSpawned()
        {
        }

        public void OnDespawned()
        {
        }

        public class Factory : PlaceholderFactory<CreaturePrefab, CreaturePrefab> { }
    }
}