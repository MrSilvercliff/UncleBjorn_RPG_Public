using UnityEngine;

namespace _Project.Scripts.GameScene.Services.Input
{
    public interface ICameraInputListener
    {
        void OnCameraZoomInput(float zoomInput);
    }
}
