using System.Text;
using UnityEngine;
using VContainer;
using Geostorm.Core.CameraSystem;
using UnityEngine.UIElements;

namespace Geostorm.Infrastructure.CameraSystem
{
    public sealed class CameraDebugOverlay : MonoBehaviour
    {
        private readonly StringBuilder _builder = new();
        private ICameraDirector _cameraDirector;

        [SerializeField]
        private bool _isVisible = true;
        [SerializeField]
        private Vector2 _position = new(16.0f, 16.0f);
        [SerializeField]
        private Vector2 _size = new(420.0f, 180.0f);

        [Inject]
        private void Construct(ICameraDirector cameraDirector)
        {
            _cameraDirector = cameraDirector;
        }

        private void OnGUI()
        {
            if (!_isVisible || _cameraDirector == null)
            {
                return;
            }

            CameraDebugState state = _cameraDirector.GetDebugState();

            _builder.Clear();
            _builder.AppendLine("Camera System");
            _builder.AppendLine($"Current mode: {state.CurrentModeId}");
            _builder.AppendLine($"Base mode: {state.BaseModeId}");

            _builder.Append("Overrides: ");
            if (state.Overrides == null || state.Overrides.Count == 0)
            {
                _builder.AppendLine("-");
            }
            else
            {
                for (int i = 0; i<state.Overrides.Count; i++)
                {
                    if (i > 0)
                    {
                        _builder.Append(" > ");
                    }
                    _builder.Append(state.Overrides[i]);
                }
                _builder.AppendLine();
            }

            _builder.Append("Registered rigs: ");
            if (state.RegisteredModeIds == null || state.RegisteredModeIds.Count == 0)
            {
                _builder.AppendLine("-");
            }
            else
            {
                bool first = true;
                foreach (CameraModeId modeId in state.RegisteredModeIds)
                {
                    if (!first)
                    {
                        _builder.Append(", ");
                    }
                    _builder.Append(modeId);
                    first = false;
                }
                _builder.AppendLine();
            }

            GUI.Box(new Rect(_position.x, _position.y, _size.x, _size.y), _builder.ToString());
        }
    }
}