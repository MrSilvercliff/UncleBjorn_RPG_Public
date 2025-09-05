using _Project.Scripts.Project.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameScene.Creatures.Basis.Components
{
    public interface ICreatureComponentBase
    {
        CreatureComponentType ComponentType { get; }

        void Init(ICreatureComponentContainer componentContainer, ICreatureController creatureController);
    }

    public abstract class CreatureComponentBase : MonoBehaviour, ICreatureComponentBase
    {
        public CreatureComponentType ComponentType => _componentType;

        [SerializeField] private CreatureComponentType _componentType;

        public abstract void Init(ICreatureComponentContainer componentContainer, ICreatureController creatureController);
    }
}