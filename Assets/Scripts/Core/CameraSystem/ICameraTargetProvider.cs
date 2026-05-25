using UnityEngine;

namespace Geostorm.Core.CameraSystem
{
    public interface ICameraTargetProvider
    {
        bool TryGetTarget(CameraTargetType targetType, out Transform target);
    }
}