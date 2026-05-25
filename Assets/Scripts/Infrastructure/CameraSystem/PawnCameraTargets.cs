using UnityEngine;
using Geostorm.Core.CameraSystem;

namespace Geostorm.Infrastructure.CameraSystem
{
    public sealed class PawnCameraTargets : MonoBehaviour, ICameraTargetProvider
    {
        [Header("Required")]
        [SerializeField]
        private Transform _root;
        [SerializeField]
        private Transform _follow;
        [SerializeField]
        private Transform _lookAt;

        [Header("Optional")]
        [SerializeField]
        private Transform _head;
        [SerializeField]
        private Transform _aim;
        [SerializeField]
        private Transform _shoulder;

        public bool TryGetTarget(CameraTargetType targetType, out Transform target)
        {
            target = targetType switch
            {
                CameraTargetType.Root => _root != null ? _root : transform,
                CameraTargetType.Follow => _follow != null ? _follow : _root != null ? _root : transform,
                CameraTargetType.LookAt => _lookAt != null ? _lookAt : _follow != null ? _follow : _root != null ? _root : transform,
                CameraTargetType.Head => _head,
                CameraTargetType.Aim => _aim,
                CameraTargetType.Shoulder => _shoulder,
                _ => null
            };
            return target != null;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _root = transform;
        }

        private void OnValidate()
        {
            if (_root == null)
            {
                _root = transform;
            }
        }
#endif
    }
}