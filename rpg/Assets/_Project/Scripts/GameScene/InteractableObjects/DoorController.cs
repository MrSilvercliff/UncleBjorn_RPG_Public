using UnityEngine;

namespace _Project.Scripts.GameScene.InteractableObjects
{
    public class DoorController : InteractableObjectController
    {
        [Header("DOOR CONTROLLER")] 
        [SerializeField] private RotatablePartController[] _rotatableControllers;
        [SerializeField] private MovablePartController[] _movableControllers;

        public override void OnInteract()
        {
            foreach (var rotatableController in _rotatableControllers)
                rotatableController.OnInteract();

            foreach (var movableController in _movableControllers)
                movableController.OnInteract();
        }
    }
}
