namespace Geostorm.Core.Input
{
    public interface IInputProvider
    {
        InputState GetCurrentState();
        void SwitchActionMap(string mapName);
    }
}