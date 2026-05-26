using System.Collections.Generic;
using UnityEngine;
using Geostorm.Core.CameraSystem;
using Geostorm.Core.CharacterSystem;
using Geostorm.Core.Input;

namespace Geostorm.Infrastructure.CharacterSystem
{
    public class PlayerBrain : ICharacterBrain
    {
        private readonly IInputProvider _inputProvider;
        private readonly IViewFrameProvider _viewFrameProvider;
        private bool _isActive;

        private const float MinMoveDirectionSqrMagnitude = 0.0001f;

        public PlayerBrain(IInputProvider inputProvider, IViewFrameProvider viewFrameProvider)
        {
            _inputProvider = inputProvider;
            _viewFrameProvider = viewFrameProvider;
        }

        public void OnPossess(IPawn targetPawn, object brainContext = null)
        {
            _isActive = true;
        }

        public void OnUnpossess()
        {
            _isActive = false;
        }

        public void GenerateCommands(List<ICommand> commandBuffer)
        {
            if (!_isActive || _inputProvider == null)
            {
                return;
            }

            InputState currentState = _inputProvider.GetCurrentState();

            Vector2 moveInput = currentState.MoveInput;
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