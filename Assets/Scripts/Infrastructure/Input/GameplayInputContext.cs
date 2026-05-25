using Geostorm.Core.Input;

namespace Geostorm.Infrastructure.Input
{
    public sealed class GameplayInputContext : IInputContext
    {
        public string ActionMapName => "Player";
        public bool ConsumeInput => false;

        public void OnActivated() { }

        public void OnDeactivated() { }
    }
}