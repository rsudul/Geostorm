using System;
using UnityEngine;

namespace Geostorm.Core.CameraSystem
{
    public sealed class CameraDirector : ICameraDirector, IViewFrameProvider
    {
        private readonly CameraRigRegistry _rigRegistry;
        private readonly CameraStack _cameraStack;

        private ICameraRig _activeRig;
        private ICameraTargetProvider _targetProvider;

        public CameraModeId CurrentModeId => _cameraStack.ActiveModeId;

        public Vector3 ViewForward
        {
            get
            {
                if (_activeRig is IViewFrameProvider viewOrientationProvider)
                {
                    return viewOrientationProvider.ViewForward;
                }
                return Vector3.forward;
            }
        }

        public Vector3 ViewRight
        {
            get
            {
                if (_activeRig is IViewFrameProvider viewOrientationProvider)
                {
                    return viewOrientationProvider.ViewRight;
                }
                return Vector3.right;
            }
        }

        public CameraDirector(CameraRigRegistry rigRegistry, CameraStack cameraStack)
        {
            _rigRegistry = rigRegistry ?? throw new ArgumentNullException(nameof(rigRegistry));
            _cameraStack = cameraStack ?? throw new ArgumentNullException(nameof(cameraStack));
        }

        public void SetTargetProvider(ICameraTargetProvider targetProvider)
        {
            _targetProvider = targetProvider;
            if (_activeRig != null)
            {
                _activeRig.BindTargetProvider(_targetProvider);
            }
        }

        public void SetBaseMode(CameraModeId modeId, CameraTransitionRequest transitionRequest)
        {
            _cameraStack.SetBaseMode(modeId);
            ApplyActiveMode(transitionRequest);
        }

        public void PushOverride(CameraModeId modeId, CameraTransitionRequest transitionRequest)
        {
            _cameraStack.PushOverride(modeId);
            ApplyActiveMode(transitionRequest);
        }

        public void PopOverride(CameraModeId modeId, CameraTransitionRequest transitionRequest)
        {
            _cameraStack.PopOverride(modeId);
            ApplyActiveMode(transitionRequest);
        }

        public void ClearOverrides(CameraTransitionRequest transitionRequest)
        {
            _cameraStack.ClearOverrides();
            ApplyActiveMode(transitionRequest);
        }

        public void Tick(float deltaTime)
        {
            _activeRig?.Tick(deltaTime);
        }

        public CameraDebugState GetDebugState()
        {
            return new CameraDebugState(_cameraStack.ActiveModeId, _cameraStack.BaseModeId, _cameraStack.Overrides, _rigRegistry.RegisteredModeIds);
        }

        private void ApplyActiveMode(CameraTransitionRequest transitionRequest)
        {
            CameraModeId activeModeId = _cameraStack.ActiveModeId;
            if (!activeModeId.IsValid)
            {
                DeactivateCurrentRig(transitionRequest);
                return;
            }

            if (!_rigRegistry.TryGetRig(activeModeId, out ICameraRig nextRig))
            {
                throw new InvalidOperationException($"Camera rig for mode '{activeModeId}' is not registered.");
            }

            if (ReferenceEquals(_activeRig, nextRig))
            {
                _activeRig.BindTargetProvider(_targetProvider);
                return;
            }

            DeactivateCurrentRig(transitionRequest);

            _activeRig = nextRig;
            _activeRig.BindTargetProvider(_targetProvider);
            _activeRig.Activate(transitionRequest);
        }

        private void DeactivateCurrentRig(CameraTransitionRequest transitionRequest)
        {
            if (_activeRig == null)
            {
                return;
            }
            _activeRig.Deactivate(transitionRequest);
            _activeRig = null;
        }
    }
}