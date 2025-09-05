using _Project.Scripts.Project.Dotween;
using _Project.Scripts.Project.Monobeh;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.GameScene.InteractableObjects
{
    public class RotatablePartController : ProjectMonoBehaviour
    {
        [Header("ROTATABLE PART CONTROLLER")]
        [SerializeField] private Transform _rotateAnchor;
        [SerializeField] private Vector3 _rotateDegrees;
        [SerializeField] private float _rotateDuration;
        [SerializeField] private Ease _rotateEase;
        [SerializeField] private bool _toggleRotation;
        [SerializeField] private Collider[] _colliders;

        private Sequence _sequence;
        
        protected override void OnAwake()
        {
            _rotateAnchor.localRotation = Quaternion.identity;
        }

        public void OnInteract()
        {
            CreateSequence();
            _sequence.Play();
        }

        private void CreateSequence()
        {
            Vector3 targetRotation;
            
            DotweenHelper.KillSequence(_sequence);
            _sequence = DOTween.Sequence();

            _sequence.OnStart(() =>
            {
                foreach (var colliderComponent in _colliders)
                    colliderComponent.enabled = false;
            });
            
            if (_toggleRotation)
                targetRotation = _rotateAnchor.localEulerAngles == _rotateDegrees ? Vector3.zero : _rotateDegrees;
            else 
                targetRotation = _rotateAnchor.localEulerAngles + _rotateDegrees;
            
            _sequence.OnComplete(() =>
            {
                foreach (var colliderComponent in _colliders)
                    colliderComponent.enabled = true;
            });
            
            _sequence.Append(_rotateAnchor.DOLocalRotate(targetRotation, _rotateDuration)).SetEase(_rotateEase);
        }
    }
}
