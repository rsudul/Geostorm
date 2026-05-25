namespace Geostorm.Core.CharacterSystem
{
    public interface ICommand
    {
        float ExpirationTime { get; }
        bool Execute(IPawn pawn);
        void Release();
    }
}