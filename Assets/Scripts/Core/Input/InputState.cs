using UnityEngine;

namespace Geostorm.Core.Input
{
    public readonly struct InputState
    {
        public readonly Vector2 MoveInput;
        public readonly Vector2 LookInput;
        public readonly bool SwitchCameraPressed;
        public readonly bool NextCharacterPressed;

        public InputState(Vector2 moveInput, Vector2 lookInput, bool switchCameraPressed, bool nextCharacterPressed)
        {
            MoveInput = moveInput;
            LookInput = lookInput;
            SwitchCameraPressed = switchCameraPressed;
            NextCharacterPressed = nextCharacterPressed;
        }
    }
}