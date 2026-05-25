using System;

namespace Geostorm.Core.Input
{
    public interface IInputProvider
    {
        InputState GetCurrentState();
        event EventHandler<NextCharacterEventArgs> OnNextCharacter;
        void SwitchActionMap(string mapName);
    }
}