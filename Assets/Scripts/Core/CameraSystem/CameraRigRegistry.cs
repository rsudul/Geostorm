using System;
using System.Collections.Generic;

namespace Geostorm.Core.CameraSystem
{
    public sealed class CameraRigRegistry
    {
        private readonly Dictionary<CameraModeId, ICameraRig> _rigs = new();

        public IReadOnlyCollection<CameraModeId> RegisteredModeIds => _rigs.Keys;

        public bool TryGetRig(CameraModeId modeId, out ICameraRig rig)
        {
            if (!modeId.IsValid)
            {
                rig = null;
                return false;
            }
            return _rigs.TryGetValue(modeId, out rig);
        }

        public void Register(ICameraRig rig)
        {
            if (rig == null)
            {
                throw new ArgumentNullException(nameof(rig));
            }

            if (!rig.ModeId.IsValid)
            {
                throw new ArgumentException("Camera rig has invalid mode id.", nameof(rig));
            }

            _rigs[rig.ModeId] = rig;
        }

        public void Unregister(ICameraRig rig)
        {
            if (rig == null)
            {
                return;
            }

            if (_rigs.TryGetValue(rig.ModeId, out ICameraRig registeredRig) && ReferenceEquals(registeredRig, rig))
            {
                _rigs.Remove(rig.ModeId);
            }
        }
    }
}