using UnityEngine;

namespace Geostorm.Core.CameraSystem
{
    public interface ICameraLookInputWriter
    {
        void SetLookInput(Vector2 lookInput);
        void Clear();
    }
}