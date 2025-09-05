using _Project.Scripts.GameScene.Services.Input;
using Assets.Plugins.ZerglingUnityPlugins.Tools.Scripts.Mono;
using UnityEngine;
using Zenject;
using ZerglingUnityPlugins.Tools.Scripts.Mono;

namespace _Project.Scripts.GameScene.Creatures.Player
{
    public class PlayerCameraController : MonoBehaviour, ICameraInputListener, IMonoLateUpdatable
    {
        [SerializeField] private Transform _anchor;
        [SerializeField] private Camera _camera;

        [Header("CAMERA ZOOM")]
        [SerializeField] private float _zoomSpeed = 0.01f;
        [SerializeField] private float _transparentZoomThreshold = 7f;
        [SerializeField] private float _minZoom = 5f;
        [SerializeField] private float _maxZoom = 20f;
        [SerializeField] private SkinnedMeshRenderer[] _meshRenderer;
        [SerializeField] private Material[] _materials;

        [Header("CAMERA COLLISION")]
        [SerializeField] private float _minDistance = 1.0f;
        [SerializeField] private float _maxDistance = 4.0f;
        [SerializeField] private float _smooth = 10.0f;
        [SerializeField] private float _smoothSpeed = 2f;
        [SerializeField] private float _distance;

        [Inject] private IMonoUpdater _monoUpdater;
        [Inject] private IInputController _inputController;

        private float _targetZoom;
        private Vector3 _initialPosition;
        private Vector3 _dollyDir;
        private float _currentOpacity = 1f;
        private float _targetOpacity;
        private float _opacityChangeTime = 0f;

        private void Awake()
        {
            _initialPosition = _camera.transform.localPosition;
            _targetZoom = _camera.transform.localPosition.z;
            _meshRenderer[0].material = _materials[0];
            _meshRenderer[1].material = _materials[1];
            _meshRenderer[2].material = _materials[2];
        }

        private void OnEnable()
        {
            _dollyDir = transform.localPosition.normalized;
            _distance = transform.localPosition.magnitude;

            _monoUpdater.Subscribe(this);
            _inputController.SubscribeListener(this);
        }

        private void OnDisable()
        {
            _monoUpdater.UnSubscribe(this);
            _inputController.UnSubscribeListener(this);
        }

        public void OnCameraZoomInput(float zoomInput)
        {
            _targetZoom += zoomInput * _zoomSpeed;
            _targetZoom = Mathf.Clamp(_targetZoom, _minZoom, _maxZoom);
        }

        public void OnLateUpdate()
        {
            DoCameraZoom();
            DoCameraCollision();
            UpdateMaterialsOpacity();
        }

        private void DoCameraZoom()
        {
            Vector3 targetPosition = new Vector3(_initialPosition.x, _initialPosition.y, _targetZoom);
            _camera.transform.localPosition = Vector3.Lerp(_camera.transform.localPosition, targetPosition, Time.deltaTime * _smoothSpeed);

            if (_camera.transform.localPosition.z >= _transparentZoomThreshold)
            {
                _targetOpacity = 0.5f;
            }
            else
            {
                _targetOpacity = 1f;
            }
            _opacityChangeTime = 0f;
        }

        private void DoCameraCollision()
        {
            Vector3 desiredCameraPos = transform.parent.TransformPoint(_dollyDir * _maxDistance);
            RaycastHit hit;

            if (Physics.Linecast(_anchor.position, desiredCameraPos, out hit))
            {
                _distance = Mathf.Clamp(hit.distance * 0.9f, _minDistance, _maxDistance);
            }
            else
            {
                _distance = _maxDistance;
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, _dollyDir * _distance, Time.deltaTime * _smooth);
        }

        private void UpdateMaterialsOpacity()
        {
            float targetOpacity = _camera.transform.localPosition.z >= _transparentZoomThreshold ? 0f : 1f;
            _currentOpacity = Mathf.Lerp(_currentOpacity, targetOpacity, Time.deltaTime * 15f);

            SetMaterialsOpacity(0, 1, 2, _currentOpacity);
        }

        private void SetMaterialsOpacity(int index1, int index2, int index3, float opacity)
        {
            SetMaterialOpacity(_materials[index1], opacity);
            SetMaterialOpacity(_materials[index2], opacity);
            SetMaterialOpacity(_materials[index3], opacity);
        }

        private void SetMaterialOpacity(Material material, float opacity)
        {
            if (material.HasProperty("_Opacity"))
            {
                float currentOpacity = material.GetFloat("_Opacity");
                float newOpacity = Mathf.Lerp(currentOpacity, opacity, Time.deltaTime * 15f);
                material.SetFloat("_Opacity", newOpacity);
            }
        }

    }
}