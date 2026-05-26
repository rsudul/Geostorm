using System;
using VContainer.Unity;
using Geostorm.Core.CameraSystem;
using Geostorm.Core.Input;

namespace Geostorm.Infrastructure.CameraSystem
{
    public sealed class CameraModeSwitchingController : IInitializable, IDisposable
    {
        private readonly IInputProvider _inputProvider;
        private readonly ICameraDirector _cameraDirector;

        public CameraModeSwitchingController(IInputProvider inputProvider, ICameraDirector cameraDirector)
        {
            _inputProvider = inputProvider;
            _cameraDirector = cameraDirector;
        }

        public void Initialize()
        {
            _inputProvider.OnSwitchCamera += HandleSwitchCamera;
        }

        public void Dispose()
        {
            _inputProvider.OnSwitchCamera -= HandleSwitchCamera;
        }

        private void HandleSwitchCamera(object sender, SwitchCameraEventArgs args)
        {
            CameraModeId currentModeId = _cameraDirector.CurrentModeId;
            CameraModeId nextModeId = currentModeId == CameraModeId.Fpp ? CameraModeId.Tpp : CameraModeId.Fpp;
            _cameraDirector.SetBaseMode(nextModeId, CameraTransitionRequest.Default);
        }
    }
}