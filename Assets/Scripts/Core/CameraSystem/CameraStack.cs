using System.Collections.Generic;

namespace Geostorm.Core.CameraSystem
{
    public sealed class CameraStack
    {
        private readonly List<CameraModeId> _overrides = new();

        public CameraModeId BaseModeId { get; private set; } = CameraModeId.None;

        public CameraModeId ActiveModeId
        {
            get
            {
                if (_overrides.Count > 0)
                {
                    return _overrides[^1];
                }
                return BaseModeId;
            }
        }

        public bool HasOverrides => _overrides.Count > 0;
        public IReadOnlyList<CameraModeId> Overrides => _overrides;

        public void SetBaseMode(CameraModeId modeId)
        {
            BaseModeId = modeId;
        }

        public void PushOverride(CameraModeId modeId)
        {
            if (!modeId.IsValid)
            {
                return;
            }
            _overrides.Add(modeId);
        }

        public void PopOverride(CameraModeId modeId)
        {
            if (!modeId.IsValid)
            {
                return;
            }

            for (int i = _overrides.Count - 1; i >= 0; i--)
            {
                if (_overrides[i] != modeId)
                {
                    continue;
                }
                _overrides.RemoveAt(i);
                return;
            }
        }

        public void ClearOverrides()
        {
            _overrides.Clear();
        }
    }
}