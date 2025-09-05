using _Project.Scripts.GameScene.Services.SpawnPoints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameScene.SpawnPoints
{
    public interface ICreatureSpawnPointController : ISpawnPointController
    { 
        string CreatureId { get; }
    }

    public class CreatureSpawnPointController : SpawnPointController, ICreatureSpawnPointController
    {
        public string CreatureId => _creatureId;

        [Header("CREATURE SPAWN POINT CONTROLLER")]
        [SerializeField] private string _creatureId;

        [Inject] private ICreatureSpawnPointRepository _creatureSpawnPointRepository;

        protected override void OnStartProcess()
        {
            base.OnStartProcess();
            _creatureSpawnPointRepository.Add(this);
        }
    }
}