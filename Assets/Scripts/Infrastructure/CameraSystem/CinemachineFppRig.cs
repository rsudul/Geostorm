using UnityEngine;
using Unity.Cinemachine;
using VContainer;
using Geostorm.Core.CameraSystem;
using Geostorm.Core.Input;

namespace Geostorm.Infrastructure.CameraSystem
{
    public sealed class CinemachineFppRig : MonoBehaviour, ICameraRig, IViewFrameProvider
    {
        private const float MinViewDirectionSqrMagnitude = 0.0001f;

        private CameraRigRegistry _rigRegistry;
        private IInputProvider _inputProvider;
        private ICameraTargetProvider _targetProvider;

        private bool _isRegistered;

        private float _targetYaw;
        private float _targetPitch;
        private float _currentYaw;
        private float _currentPitch;
        private float _yawVelocity;
        private float _pitchVelocity;

        public CameraModeId ModeId => CameraModeId.Fpp;
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
                forward.y = 0.0f;
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
                right.y = 0.0f;
                if (right.sqrMagnitude <= MinViewDirectionSqrMagnitude)
                {
                    return Vector3.right;
                }
                return right.normalized;
            }
        }

        [Header("Cinemachine")]
        [SerializeField]
        private CinemachineCamera _camera;

        [Header("Data")]
        [SerializeField]
        private FppCameraRigData _data;

        [Inject]
        private void Construct(CameraRigRegistry rigRegistry, IInputProvider inputProvider)
        {
            _rigRegistry = rigRegistry;
            _inputProvider = inputProvider;
            RegisterIfPossible();
        }

        private void Awake()
        {
            if (_camera == null)
            {
                _camera = GetComponent<CinemachineCamera>();
            }
            ResetLookToDefaults();
            ApplyRotation();
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
            ApplyRotation();
            SetPriority(GetActivePriority());
        }

        public void Deactivate(CameraTransitionRequest transitionRequest)
        {
            IsActive = false;
            SetPriority(GetInactivePriority());
        }

        public void Tick(float deltaTime)
        {
            if (!IsActive)
            {
                return;
            }

            ReadLookInput();
            SmoothLook(deltaTime);
            ApplyRotation();
        }

        private void ReadLookInput()
        {
            if (_inputProvider == null)
            {
                return;
            }

            InputState currentState = _inputProvider.GetCurrentState();
            Vector2 lookInput = currentState.LookInput;
            if (lookInput.sqrMagnitude <= 0.0f)
            {
                return;
            }

            float verticalSign = GetInvertY() ? 1.0f : -1.0f;

            _targetYaw += lookInput.x * GetHorizontalSensitivity();
            _targetPitch += lookInput.y * GetVerticalSensitivity() * verticalSign;
            _targetPitch = Mathf.Clamp(_targetPitch, GetMinPitch(), GetMaxPitch());
        }

        private void SmoothLook(float deltaTime)
        {
            float smoothTime = GetRotationSmoothTime();
            if (smoothTime <= 0.0f)
            {
                _currentYaw = _targetYaw;
                _currentPitch = _targetPitch;
                return;
            }

            _currentYaw = Mathf.SmoothDampAngle(_currentYaw, _targetYaw, ref _yawVelocity, smoothTime, Mathf.Infinity, deltaTime);
            _currentPitch = Mathf.SmoothDamp(_currentPitch, _targetPitch, ref _pitchVelocity, smoothTime, Mathf.Infinity, deltaTime);
        }

        private void ApplyTargets()
        {
            if (_camera == null || _targetProvider == null)
            {
                return;
            }

            if (_targetProvider.TryGetTarget(CameraTargetType.Head, out Transform headTarget))
            {
                _camera.Follow = headTarget;
                _camera.LookAt = null;
                return;
            }

            if (_targetProvider.TryGetTarget(CameraTargetType.LookAt, out Transform lookAtTarget))
            {
                _camera.Follow = lookAtTarget;
                _camera.LookAt = null;
            }
        }

        private void ApplyRotation()
        {
            if (_camera == null)
            {
                return;
            }
            _camera.transform.rotation = Quaternion.Euler(_currentPitch, _currentYaw, 0.0f);
        }

        private void ResetLookToDefaults()
        {
            float defaultYaw = _data != null ? _data.DefaultYaw : 0.0f;
            float defaultPitch = _data != null ? _data.DefaultPitch : 0.0f;
            defaultPitch = Mathf.Clamp(defaultPitch, GetMinPitch(), GetMaxPitch());

            _targetYaw = defaultYaw;
            _targetPitch = defaultPitch;
            _currentYaw = defaultYaw;
            _currentPitch = defaultPitch;
            _yawVelocity = 0.0f;
            _pitchVelocity = 0.0f;
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

        private float GetHorizontalSensitivity()
        {
            return _data != null ? _data.HorizontalSensitivity : 0.12f;
        }

        private float GetVerticalSensitivity()
        {
            return _data != null ? _data.VerticalSensitivity : 0.12f;
        }

        private float GetMinPitch()
        {
            return _data != null ? _data.MinPitch : -80.0f;
        }

        private float GetMaxPitch()
        {
            return _data != null ? _data.MaxPitch : 80.0f;
        }

        private bool GetInvertY()
        {
            return _data != null && _data.InvertY;
        }

        private float GetRotationSmoothTime()
        {
            return _data != null ? _data.RotationSmoothTime : 0.02f;
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

            if (_data != null && _data.MinPitch > _data.MaxPitch)
            {
                Debug.LogWarning($"{nameof(FppCameraRigData)} has MinPitch greater than MaxPitch.");
            }
        }
#endif
    }
}