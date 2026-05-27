using UnityEngine;

namespace Geostorm.Core.Input
{
    public interface IPlayerInputIntentReceiver
    {
        void SetMoveInput(Vector2 moveInput);
        void SetLookInput(Vector2 lookInput);
        void ClearInput();
    }
}