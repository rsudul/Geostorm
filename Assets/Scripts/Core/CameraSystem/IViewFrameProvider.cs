using UnityEngine;

namespace Geostorm.Core.CameraSystem
{
    public interface IViewFrameProvider
    {
        Vector3 ViewForward { get; }
        Vector3 ViewRight { get; }
    }
}