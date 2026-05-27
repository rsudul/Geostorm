using VContainer.Unity;
using Geostorm.Core.Input;
using Geostorm.Infrastructure.CharacterSystem;
using Geostorm.Infrastructure.Input;

namespace Geostorm.Infrastructure
{
    public class TestLevelInitializer : IStartable
    {
        private readonly PlayerPossessionController _playerPossessionController;
        private readonly InputRouter _inputRouter;

        public TestLevelInitializer(PlayerPossessionController playerPossessionController, InputRouter inputRouter)
        {
            _playerPossessionController = playerPossessionController;
            _inputRouter = inputRouter;
        }

        public void Start()
        {
            _playerPossessionController.InitializePlayerPossession();
            _inputRouter.PushContext(new GameplayInputContext());
        }
    }
}