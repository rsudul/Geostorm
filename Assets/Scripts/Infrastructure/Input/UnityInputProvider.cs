using System;
using UnityEngine;
using Geostorm.Core.Input;

namespace Geostorm.Infrastructure.Input
{
    public sealed class UnityInputProvider : IInputProvider, IDisposable
    {
        private readonly InputSystem_Actions _inputActions;

        public UnityInputProvider()
        {
            _inputActions = new InputSystem_Actions();
            _inputActions.Disable();
        }

        public InputState GetCurrentState()
        {
            Vector2 move = _inputActions.Player.Move.ReadValue<Vector2>();
            Vector2 look = _inputActions.Player.Look.ReadValue<Vector2>();
            bool switchCameraPressed = _inputActions.Player.SwitchCamera.WasPressedThisFrame();
            bool nextCharacterPressed = _inputActions.Player.Next.WasPressedThisFrame();
            return new InputState(move, look, switchCameraPressed, nextCharacterPressed);
        }

        public void SwitchActionMap(string mapName)
        {
            _inputActions.Disable();

            if (string.IsNullOrEmpty(mapName) || mapName == "None")
            {
                return;
            }

            var map = _inputActions.asset.FindActionMap(mapName);
            if (map != null)
            {
                map.Enable();
            }
            else
            {
                Debug.LogWarning($"Action map '{mapName}' not found.");
            }
        }

        public void Dispose()
        {
            _inputActions?.Dispose();
        }
    }
}