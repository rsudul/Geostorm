using UnityEngine;

namespace Geostorm.Core.CameraSystem
{
    public sealed class CameraLookInputState : ICameraLookInput, ICameraLookInputWriter
    {
        public Vector2 LookInput { get; private set; }

        public void SetLookInput(Vector2 lookInput)
        {
            LookInput = lookInput;
        }

        public void Clear()
        {
            LookInput = Vector2.zero;
        }
    }
}