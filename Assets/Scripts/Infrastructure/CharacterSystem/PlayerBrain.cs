using System.Collections.Generic;
using UnityEngine;
using Geostorm.Core.CharacterSystem;
using Geostorm.Core.Input;

namespace Geostorm.Infrastructure.CharacterSystem
{
    public class PlayerBrain : ICharacterBrain
    {
        private IInputProvider _inputProvider;
        private bool _isActive;

        public PlayerBrain(IInputProvider inputProvider)
        {
            _inputProvider = inputProvider;
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

            Vector3 direction = new Vector3(currentState.MoveInput.x, 0.0f, currentState.MoveInput.y);
            if (direction.sqrMagnitude > 1.0f)
            {
                direction.Normalize();
            }

            if (direction != Vector3.zero)
            {
                commandBuffer.Add(MoveCommand.Get(direction));
            }
        }
    }
}