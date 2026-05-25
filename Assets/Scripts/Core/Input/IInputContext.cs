namespace Geostorm.Core.Input
{
    public interface IInputContext
    {
        string ActionMapName { get; }
        bool ConsumeInput { get; }
        void OnActivated();
        void OnDeactivated();
    }
}