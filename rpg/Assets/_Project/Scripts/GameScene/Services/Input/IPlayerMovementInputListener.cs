using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.GameScene.Services.Input
{
    public interface IPlayerMovementInputListener
    {
        void OnMoveInput(InputActionPhase phase, Vector3 moveInput);
        void OnFreeLookInput(InputActionPhase phase);
        void OnRotateLookInput(InputActionPhase phase);
        void OnLookInput(InputActionPhase phase, Vector2 lookInput);
        void OnJumpInput(InputActionPhase phase);
    }
}