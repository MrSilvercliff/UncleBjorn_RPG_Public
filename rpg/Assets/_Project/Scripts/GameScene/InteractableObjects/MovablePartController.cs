using _Project.Scripts.Project.Dotween;
using _Project.Scripts.Project.Monobeh;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.GameScene.InteractableObjects
{
    public class MovablePartController : ProjectMonoBehaviour
    {
        [Header("MOVABLE PART CONTROLLER")]
        [SerializeField] private Transform _moveAnchor;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;
        [SerializeField] private float _moveDuration;
        [SerializeField] private Ease _moveEase;
        [SerializeField] private Collider[] _colliders;

        private Transform _currentPoint;
        private Sequence _sequence;
        
        protected override void OnAwake()
        {
            base.OnAwake();
            _moveAnchor.localPosition = _startPoint.localPosition;
            _currentPoint = _startPoint;
        }

        public void OnInteract()
        {
            CreateSequence();
            _sequence.Play();
        }

        private void CreateSequence()
        {
            var targetPoint = _currentPoint == _startPoint ? _endPoint : _startPoint;
            
            DotweenHelper.KillSequence(_sequence);
            _sequence = DOTween.Sequence();
            
            _sequence.OnStart(() =>
            {
                foreach (var colliderComponent in _colliders)
                    colliderComponent.enabled = false;
                
                _currentPoint = targetPoint;
            });

            _sequence.Append(_moveAnchor.DOLocalMove(targetPoint.localPosition, _moveDuration)).SetEase(_moveEase);

            _sequence.OnComplete(() =>
            {
                foreach (var colliderComponent in _colliders)
                    colliderComponent.enabled = true;
            });
        }
    }
}
