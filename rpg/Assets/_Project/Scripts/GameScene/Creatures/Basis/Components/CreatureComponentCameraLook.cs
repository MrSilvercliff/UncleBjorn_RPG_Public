using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameScene.Creatures.Basis.Components
{
    public interface ICreatureComponentCameraLook : ICreatureComponentBase
    {
        Transform CameraTransform { get; }
        Transform CreatureTransform { get; }
    }

    public class CreatureComponentCameraLook : CreatureComponentBase, ICreatureComponentCameraLook
    {
        public Transform CameraTransform => _cameraTransform;
        public Transform CreatureTransform => _creatureTransform;

        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _creatureTransform;

        public override void Init(ICreatureComponentContainer componentContainer, ICreatureController creatureController)
        {
        }
    }
}