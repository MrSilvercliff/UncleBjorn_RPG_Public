using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameScene.Creatures
{
    public interface ICreatureHelper
    {
        bool IsGrounded(Vector3 creaturePosition, Vector3 sphereOffset, float sphereRadius, LayerMask groundLayerMask, QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore);
    }

    public class CreatureHelper : ICreatureHelper
    {
        public bool IsGrounded(Vector3 creaturePosition, Vector3 sphereOffset, float sphereRadius, LayerMask groundLayerMask, 
            QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore)
        {
            var spherePosition = new Vector3(creaturePosition.x + sphereOffset.x, 
                creaturePosition.y + sphereOffset.y, 
                creaturePosition.z + sphereOffset.z);

            var result = Physics.CheckSphere(spherePosition, sphereRadius, groundLayerMask, triggerInteraction);
            return result;
        }
    }
}