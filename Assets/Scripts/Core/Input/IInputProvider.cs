using System;

namespace Geostorm.Core.Input
{
    public interface IInputProvider
    {
        event EventHandler<NextCharacterEventArgs> OnNextCharacter;
        event EventHandler<SwitchCameraEventArgs> OnSwitchCamera;
        InputState GetCurrentState();
        void SwitchActionMap(string mapName);
    }
}