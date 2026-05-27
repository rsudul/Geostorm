using System.Collections.Generic;
using UnityEngine;
using Geostorm.Core.CameraSystem;
using Geostorm.Core.CharacterSystem;
using Geostorm.Core.Input;

namespace Geostorm.Infrastructure.CharacterSystem
{
    public class PlayerBrain : ICharacterBrain, IPlayerInputIntentReceiver
    {
        private readonly IViewFrameProvider _viewFrameProvider;
        private readonly ICameraDirector _cameraDirector;

        private bool _isActive;
        private Vector2 _moveInput;
        private Vector2 _lookInput;

        private PawnRotationMode _currentRotationMode = PawnRotationMode.MovementDirection;
        private bool _forceApplyRotationMode;

        private const float MinMoveDirectionSqrMagnitude = 0.0001f;
        private const float MinYawInputAbs = 0.0001f;

        public Vector2 LookInput => _lookInput;

        public PlayerBrain(IViewFrameProvider viewFrameProvider, ICameraDirector cameraDirector)
        {
            _viewFrameProvider = viewFrameProvider;
            _cameraDirector = cameraDirector;
        }

        public void OnPossess(IPawn targetPawn, object brainContext = null)
        {
            _isActive = true;
            _forceApplyRotationMode = true;
        }

        public void OnUnpossess()
        {
            _isActive = false;
            ClearInput();
            _forceApplyRotationMode = true;
        }

        public void SetMoveInput(Vector2 moveInput)
        {
            _moveInput = moveInput;
        }

        public void SetLookInput(Vector2 lookInput)
        {
            _lookInput = lookInput;
        }

        public void ClearInput()
        {
            _moveInput = Vector2.zero;
            _lookInput = Vector2.zero;
        }

        public void GenerateCommands(List<ICommand> commandBuffer)
        {
            if (!_isActive)
            {
                return;
            }

            bool isFpp = _cameraDirector.CurrentModeId == CameraModeId.Fpp;
            PawnRotationMode desiredRotationMode = isFpp ? PawnRotationMode.ManualYaw : PawnRotationMode.MovementDirection;

            if (_forceApplyRotationMode || desiredRotationMode != _currentRotationMode)
            {
                _currentRotationMode = desiredRotationMode;
                _forceApplyRotationMode = false;
                commandBuffer.Add(SetRotationModeCommand.Get(_currentRotationMode));
            }

            if (isFpp && Mathf.Abs(_lookInput.x) > MinYawInputAbs)
            {
                commandBuffer.Add(YawInputCommand.Get(_lookInput.x));
            }

            GenerateMovementCommand(commandBuffer);
        }

        private void GenerateMovementCommand(List<ICommand> commandBuffer)
        {
            Vector2 moveInput = _moveInput;
            if (moveInput.sqrMagnitude > 1.0f)
            {
                moveInput.Normalize();
            }

            Vector3 viewForward = GetFlattenedDirection(_viewFrameProvider.ViewForward, Vector3.forward);
            Vector3 viewRight = GetFlattenedDirection(_viewFrameProvider.ViewRight, Vector3.right);

            Vector3 direction = viewRight * moveInput.x + viewForward * moveInput.y;
            if (direction.sqrMagnitude <= MinMoveDirectionSqrMagnitude)
            {
                return;
            }
            direction.Normalize();
            commandBuffer.Add(MoveCommand.Get(direction));
        }

        private static Vector3 GetFlattenedDirection(Vector3 direction, Vector3 fallback)
        {
            direction.y = 0.0f;
            if (direction.sqrMagnitude <= MinMoveDirectionSqrMagnitude)
            {
                return fallback;
            }
            return direction.normalized;
        }
    }
}