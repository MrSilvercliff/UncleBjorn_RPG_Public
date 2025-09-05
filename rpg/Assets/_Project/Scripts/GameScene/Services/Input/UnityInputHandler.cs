using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Project.Scripts.GameScene.Services.Input
{
    public class UnityInputHandler : MonoBehaviour, IInputHandler
    {
        [Inject] private IInputController _inputController;

        private Vector3 _moveInput;
        private Vector2 _lookInput;

        public Task<bool> Init()
        {
            _moveInput = new Vector3();
            gameObject.SetActive(true);
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            return true;
        }

        public void OnMoveInputAction(InputAction.CallbackContext context)
        { 
            var inputVector = context.ReadValue<Vector2>();
            _moveInput.x = inputVector.x;
            _moveInput.z = inputVector.y;
            _inputController.OnMoveInput(context.phase, _moveInput);
        }

        public void OnFreeLookInputAction(InputAction.CallbackContext context)
        {
            //Debug.Log($"OnFreeLookInputAction = {context.phase}");
            _inputController.OnFreeLookInput(context.phase);
        }

        public void OnRotateLookInputAction(InputAction.CallbackContext context)
        {
            //Debug.Log($"OnRotateLookInputAction = {context.phase}");
            _inputController.OnRotateLookInput(context.phase);
        }

        public void OnLookInputAction(InputAction.CallbackContext context)
        {
            _lookInput = context.ReadValue<Vector2>();
            //Debug.Log($"OnLookInputAction = {context.phase} ; {_lookInput}");
            _inputController.OnLookInput(context.phase, _lookInput);
        }

        public void OnJumpInputAction(InputAction.CallbackContext context)
        { 
            _inputController.OnJumpInput(context.phase);
        }

        public void OnCameraZoomInputAction(InputAction.CallbackContext context)
        {
            float zoomInput = context.ReadValue<float>();
            _inputController.OnCameraZoomInput(zoomInput);
        }
    }
}