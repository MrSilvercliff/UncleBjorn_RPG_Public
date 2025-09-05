using System;
using _Project.Scripts.GameScene.Enums;
using _Project.Scripts.GameScene.Services.InteractableObjects;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Project.Monobeh;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Project.Scripts.GameScene.InteractableObjects
{
    public interface IInteractableObjectController : IProjectMonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    { 
        InteractableObjectType ObjectType { get; }

        void SetSelected(bool selected);
        void OnInteract();
    }

    public class InteractableObjectController : ProjectMonoBehaviour, IInteractableObjectController
    {
        public InteractableObjectType ObjectType => _objectType;

        [Header("ITNERACTABLE OBJECT CONTROLLER")]
        [SerializeField] private InteractableObjectType _objectType;
        [SerializeField] private MeshRenderer[] _meshRenderers;
        [SerializeField] private GameObject _selectionObject;
        [SerializeField] private bool _canBeSelectedAsTarget;
        [SerializeField] protected bool _interactable;

        [Inject] private IInteractableObjectService _interactableObjectService;

        private void OnEnable()
        {
            OnEnableProcess();
        }

        protected virtual void OnEnableProcess()
        {
        }

        private void OnDisable()
        {
            OnDisableProcess();
        }

        protected virtual void OnDisableProcess()
        {
        }

        public void SetSelected(bool selected)
        {
            _selectionObject.SetActive(selected);
        }

        private void SetHighlighted(bool highlighted)
        {
            var materialValue = highlighted ? InteractableObjectHelper.HIGHLIGHT_POINTER_ENTER_VALUE : InteractableObjectHelper.HIGHLIGHT_POINTER_EXIT_VALUE;

            foreach (var meshRenderer in _meshRenderers)
            {
                foreach (var material in meshRenderer.materials)
                    material.SetFloat(InteractableObjectHelper.MATERIAL_HIGHLIGHT_FLOAT_NAME, materialValue);
            }
        }

        public void OnPointerClick(PointerEventData eventData) 
        {
            if (!_interactable)
                return;
            
            if (_canBeSelectedAsTarget)
                _interactableObjectService.Select(this);

            if (eventData.button != PointerEventData.InputButton.Right)
                return;
            
            _interactableObjectService.TryInteract(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_interactable)
                return;
            
            SetHighlighted(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetHighlighted(false);
        }

        public virtual void OnInteract()
        {
        }
    }
}