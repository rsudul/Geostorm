using UnityEngine;
using Unity.Cinemachine;
using VContainer;
using Geostorm.Core.CameraSystem;

namespace Geostorm.Infrastructure.CameraSystem
{
    public sealed class CinemachineTppRig : MonoBehaviour, ICameraRig, IViewFrameProvider
    {
        private CameraRigRegistry _rigRegistry;
        private ICameraTargetProvider _targetProvider;
        private bool _isRegistered;

        private const float MinViewDirectionSqrMagnitude = 0.0001f;

        [Header("Cinemachine")]
        [SerializeField]
        private CinemachineCamera _camera;

        [Header("Data")]
        [SerializeField]
        private TppCameraRigData _data;

        public CameraModeId ModeId => CameraModeId.Tpp;
        public bool IsActive { get; private set; }

        public Vector3 ViewForward
        {
            get
            {
                if (_camera == null)
                {
                    return Vector3.forward;
                }

                Vector3 forward = _camera.transform.forward;

                if (_data != null && _data.FlattenViewFrame)
                {
                    forward.y = 0.0f;
                }

                if (forward.sqrMagnitude <= MinViewDirectionSqrMagnitude)
                {
                    return Vector3.forward;
                }

                return forward.normalized;
            }
        }

        public Vector3 ViewRight
        {
            get
            {
                if (_camera == null)
                {
                    return Vector3.right;
                }

                Vector3 right = _camera.transform.right;

                if (_data != null && _data.FlattenViewFrame)
                {
                    right.y = 0.0f;
                }

                if (right.sqrMagnitude < MinViewDirectionSqrMagnitude)
                {
                    return Vector3.right;
                }

                return right.normalized;
            }
        }

        [Inject]
        private void Construct(CameraRigRegistry rigRegistry)
        {
            _rigRegistry = rigRegistry;
            RegisterIfPossible();
        }

        private void Awake()
        {
            if (_camera == null)
            {
                _camera = GetComponent<CinemachineCamera>();
            }
        }

        private void OnEnable()
        {
            SetPriority(GetInactivePriority());
            RegisterIfPossible();
        }

        private void OnDisable()
        {
            UnregisterIfNeeded();
        }

        public void BindTargetProvider(ICameraTargetProvider targetProvider)
        {
            _targetProvider = targetProvider;
            ApplyTargets();
        }

        public void Activate(CameraTransitionRequest transitionRequest)
        {
            IsActive = true;
            ApplyTargets();
            SetPriority(GetActivePriority());
        }

        public void Deactivate(CameraTransitionRequest transitionRequest)
        {
            IsActive = false;
            SetPriority(GetInactivePriority());
        }

        public void Tick(float deltaTime)
        {

        }

        private void ApplyTargets()
        {
            if (_camera == null || _targetProvider == null)
            {
                return;
            }

            if (_targetProvider.TryGetTarget(CameraTargetType.Follow, out Transform followTarget))
            {
                _camera.Follow = followTarget;
            }

            if (_targetProvider.TryGetTarget(CameraTargetType.LookAt, out Transform lookAtTarget))
            {
                _camera.LookAt = lookAtTarget;
            }
        }

        private void RegisterIfPossible()
        {
            if (_isRegistered || _rigRegistry == null)
            {
                return;
            }
            _rigRegistry.Register(this);
            _isRegistered = true;
        }

        private void UnregisterIfNeeded()
        {
            if (!_isRegistered || _rigRegistry == null)
            {
                return;
            }
            _rigRegistry.Unregister(this);
            _isRegistered = false;
        }

        private void SetPriority(int priority)
        {
            if (_camera == null)
            {
                return;
            }
            _camera.Priority = priority;
        }

        private int GetActivePriority()
        {
            return _data != null ? _data.ActivePriority : 100;
        }

        private int GetInactivePriority()
        {
            return _data != null ? _data.InactivePriority : 0;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _camera = GetComponent<CinemachineCamera>();
        }

        private void OnValidate()
        {
            if (_camera == null)
            {
                _camera = GetComponent<CinemachineCamera>();
            }
        }
#endif
    }
}