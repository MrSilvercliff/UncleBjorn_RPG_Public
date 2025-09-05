using _Project.Scripts.GameScene.InteractableObjects;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.GameScene.Services.Creatures;
using _Project.Scripts.Project.Services.Balance;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameScene.Services.InteractableObjects
{
    public interface IInteractableObjectService
    {
        IInteractableObjectController SelectedObject { get; }

        void Select(IInteractableObjectController objectController);
        void SetInteractBlock(bool blocked);
        void TryInteract(IInteractableObjectController objectController);
    }

    public class InteractableObjectService : IInteractableObjectService
    {
        public IInteractableObjectController SelectedObject => _selectedObject;

        [Inject] private ICreatureControllerRepository _creatureControllerRepository;
        [Inject] private IProjectBalanceStorage _balanceStorage;

        private IInteractableObjectController _selectedObject;

        private bool _interactBlocked;

        public InteractableObjectService()
        {
            _selectedObject = null;
            _interactBlocked = false;
        }

        public void Select(IInteractableObjectController objectController)
        {
            _selectedObject?.SetSelected(false);
            _selectedObject = objectController;
            _selectedObject?.SetSelected(true);
        }

        public void SetInteractBlock(bool blocked)
        {
            _interactBlocked = blocked;
        }

        public void TryInteract(IInteractableObjectController objectController)
        {
            if (_interactBlocked)
                return;

            var checkRangeResult = CheckInteractRange(objectController);

            if (!checkRangeResult)
                return;
            
            objectController.OnInteract();
        }

        private bool CheckInteractRange(IInteractableObjectController objectController)
        {
            var playerController = _creatureControllerRepository.PlayerController;
            var distanceVector = objectController.Transform.position - playerController.Transform.position;
            var interactRange = _balanceStorage.InteractableObjects.InteractMaxRange;
            var interactRangeSqr = interactRange * interactRange;

            if (distanceVector.sqrMagnitude > interactRangeSqr)
                return false;

            return true;
        }
    }
}