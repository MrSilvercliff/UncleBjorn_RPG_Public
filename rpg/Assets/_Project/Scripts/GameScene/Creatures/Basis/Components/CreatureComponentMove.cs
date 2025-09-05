using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameScene.Creatures.Basis.Components
{
    public interface ICreatureComponentMove : ICreatureComponentBase
    { 
        Rigidbody RigidBody { get; }
    }

    public class CreatureComponentMove : CreatureComponentBase, ICreatureComponentMove
    {
        public Rigidbody RigidBody => _rigidBody;

        [SerializeField] private Rigidbody _rigidBody;

        public override void Init(ICreatureComponentContainer componentContainer, ICreatureController creatureController)
        {
        }
    }
}