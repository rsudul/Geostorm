using VContainer.Unity;
using Geostorm.Core.CameraSystem;
using Geostorm.Core.Input;
using Geostorm.Infrastructure.CharacterSystem;
using Geostorm.Core.CharacterSystem;

namespace Geostorm.Infrastructure.Input
{
    public sealed class GameplayInputController : ITickable
    {
        private readonly IInputProvider _inputProvider;
        private readonly IPlayerInputIntentReceiver _playerInputIntentReceiver;
        private readonly ICameraLookInputWriter _cameraLookInputWriter;
        private readonly ICameraModeSwitcher _cameraModeSwitcher;
        private readonly IPlayerCharacterSwitcher _playerCharacterSwitcher;

        public GameplayInputController(
            IInputProvider inputProvider,
            IPlayerInputIntentReceiver playerInputIntentReceiver,
            ICameraLookInputWriter cameraLookInputWriter,
            ICameraModeSwitcher cameraModeSwitcher,
            IPlayerCharacterSwitcher playerCharacterSwitcher
        )
        {
            _inputProvider = inputProvider;
            _playerInputIntentReceiver = playerInputIntentReceiver;
            _cameraLookInputWriter = cameraLookInputWriter;
            _cameraModeSwitcher = cameraModeSwitcher;
            _playerCharacterSwitcher = playerCharacterSwitcher;
        }

        public void Tick()
        {
            InputState inputState = _inputProvider.GetCurrentState();

            _playerInputIntentReceiver.SetMoveInput(inputState.MoveInput);
            _playerInputIntentReceiver.SetLookInput(inputState.LookInput);
            _cameraLookInputWriter.SetLookInput(inputState.LookInput);

            if (inputState.SwitchCameraPressed)
            {
                _cameraModeSwitcher.SwitchNext();
            }

            if (inputState.NextCharacterPressed)
            {
                _playerCharacterSwitcher.SwitchToNextCharacter();
            }
        }
    }
}