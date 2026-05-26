using System.Collections.Generic;

namespace Geostorm.Core.CameraSystem
{
    public readonly struct CameraDebugState
    {
        public CameraModeId CurrentModeId { get; }
        public CameraModeId BaseModeId { get; }
        public IReadOnlyList<CameraModeId> Overrides { get; }
        public IReadOnlyCollection<CameraModeId> RegisteredModeIds { get; }

        public CameraDebugState(CameraModeId currentModeId, CameraModeId baseModeId, IReadOnlyList<CameraModeId> overrides, IReadOnlyCollection<CameraModeId> registeredModeIds)
        {
            CurrentModeId = currentModeId;
            BaseModeId = baseModeId;
            Overrides = overrides;
            RegisteredModeIds = registeredModeIds;
        }
    }
}