using UnityEngine;

namespace Geostorm.Core.Input
{
    public readonly struct InputState
    {
        public readonly Vector2 MoveInput;
        public readonly Vector2 LookInput;

        public InputState(Vector2 moveInput, Vector2 lookInput)
        {
            MoveInput = moveInput;
            LookInput = lookInput;
        }
    }
}