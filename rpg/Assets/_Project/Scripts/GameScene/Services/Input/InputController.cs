using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using ZerglingUnityPlugins.Tools.Scripts.Interfaces.ProjectService.AsyncSync;

namespace _Project.Scripts.GameScene.Services.Input
{
    public interface IInputController : IProjectSerivce, IPlayerMovementInputListener, ICameraInputListener, IUIInputListener
    {
        Vector3 MoveInput { get; }

        void SubscribeListener(IPlayerMovementInputListener listener);
        void UnSubscribeListener(IPlayerMovementInputListener listener);

        void SubscribeListener(ICameraInputListener listener);
        void UnSubscribeListener(ICameraInputListener listener);

        void SubscribeListener(IUIInputListener listener);
        void UnSubscribeListener(IUIInputListener listener);
    }

    public class InputController : IInputController
    {
        public Vector3 MoveInput => _moveInput;

        private Vector3 _moveInput;

        private HashSet<IPlayerMovementInputListener> _playerInputListeners;
        private HashSet<ICameraInputListener> _cameraInputListeners;
        private HashSet<IUIInputListener> _uiInputListeners;

        public InputController()
        {
            _moveInput = Vector3.zero;
            _playerInputListeners = new();
            _cameraInputListeners = new();
            _uiInputListeners = new();
        }

        public Task<bool> Init()
        {
            return Task.FromResult(true);
        }

        public bool Flush()
        {
            return true;
        }

        public void SubscribeListener(IPlayerMovementInputListener listener)
        {
            _playerInputListeners.Add(listener);
        }

        public void UnSubscribeListener(IPlayerMovementInputListener listener)
        {
            _playerInputListeners.Remove(listener);
        }

        public void SubscribeListener(ICameraInputListener listener)
        {
            _cameraInputListeners.Add(listener);
        }

        public void UnSubscribeListener(ICameraInputListener listener)
        {
            _cameraInputListeners.Remove(listener);
        }

        public void SubscribeListener(IUIInputListener listener)
        {
            _uiInputListeners.Add(listener);
        }

        public void UnSubscribeListener(IUIInputListener listener)
        {
            _uiInputListeners.Remove(listener);
        }

        #region PlayerMovement

        public void OnMoveInput(InputActionPhase phase, Vector3 moveInput)
        {
            _moveInput = moveInput;

            foreach (var listener in _playerInputListeners)
                listener.OnMoveInput(phase, moveInput);
        }

        public void OnFreeLookInput(InputActionPhase phase)
        {
            foreach (var listener in _playerInputListeners)
                listener.OnFreeLookInput(phase);
        }

        public void OnRotateLookInput(InputActionPhase phase)
        {
            foreach (var listener in _playerInputListeners)
                listener.OnRotateLookInput(phase);
        }

        public void OnLookInput(InputActionPhase phase, Vector2 lookInput)
        {
            foreach (var listener in _playerInputListeners)
                listener.OnLookInput(phase, lookInput);
        }

        public void OnJumpInput(InputActionPhase phase)
        {
            foreach (var listener in _playerInputListeners)
                listener.OnJumpInput(phase);
        }

        #endregion

        #region CameraInput

        public void OnCameraZoomInput(float zoomInput)
        {
            foreach (var listener in _cameraInputListeners)
                listener.OnCameraZoomInput(zoomInput);
        }

        #endregion

        #region UIInput


        #endregion
    }
}
