using UnityEngine;

namespace Geostorm.Infrastructure.CameraSystem
{
    [CreateAssetMenu(fileName = "FppCameraRigData", menuName = "Geostorm/Camera/Fpp Camera Rig Data")]
    public sealed class FppCameraRigData : ScriptableObject
    {
        [Header("Priority")]
        [SerializeField]
        private int _activePriority = 100;
        [SerializeField]
        private int _inactivePriority = 0;

        [Header("Look")]
        [SerializeField]
        private float _defaultYaw = 0.0f;
        [SerializeField]
        private float _defaultPitch = 0.0f;
        [SerializeField]
        private float _minPitch = -80.0f;
        [SerializeField]
        private float _maxPitch = 80.0f;
        [SerializeField]
        private float _horizontalSensitivity = 0.12f;
        [SerializeField]
        private float _verticalSensitivity = 0.12f;
        [SerializeField]
        private bool _invertY = false;
        [SerializeField]
        private float _rotationSmoothTime = 0.02f;

        public int ActivePriority => _activePriority;
        public int InactivePriority => _inactivePriority;

        public float DefaultYaw => _defaultYaw;
        public float DefaultPitch => _defaultPitch;
        public float MinPitch => _minPitch;
        public float MaxPitch => _maxPitch;
        public float HorizontalSensitivity => _horizontalSensitivity;
        public float VerticalSensitivity => _verticalSensitivity;
        public bool InvertY => _invertY;
        public float RotationSmoothTime => _rotationSmoothTime;
    }
}