using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameScene.Creatures.Basis.Components
{
    public interface ICreatureComponentCharacterController : ICreatureComponentBase
    {
        CharacterController CharacterController { get; }
        float GravityMultiplier { get; }
        Vector3 GroundCheckOffset { get; }
        float GroundCheckRadius { get; }
        LayerMask GroundLayers { get; }
    }

    public class CreatureComponentCharacterController : CreatureComponentBase, ICreatureComponentCharacterController
    {
        public CharacterController CharacterController => _characterController;
        public float GravityMultiplier => _gravityMultiplier;
        public Vector3 GroundCheckOffset => _groundCheckOffset;
        public float GroundCheckRadius => _groundCheckRadius;
        public LayerMask GroundLayers => _groundLayers;

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _gravityMultiplier;
        [SerializeField] private Vector3 _groundCheckOffset;
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private LayerMask _groundLayers;

        [SerializeField] private bool _drawGizmos;

        public override void Init(ICreatureComponentContainer componentContainer, ICreatureController creatureController)
        {
        }

        private void OnDrawGizmos()
        {
            if (!_drawGizmos)
                return;

            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);

            var position = transform.position;
            var spherePosition = new Vector3(position.x + _groundCheckOffset.x,
                position.y + _groundCheckOffset.y, position.z + _groundCheckOffset.z);

            Gizmos.color = transparentGreen;
            Gizmos.DrawSphere(spherePosition, _groundCheckRadius);
        }
    }
}