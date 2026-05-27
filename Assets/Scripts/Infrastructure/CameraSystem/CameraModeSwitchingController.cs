using Geostorm.Core.CameraSystem;

namespace Geostorm.Infrastructure.CameraSystem
{
    public sealed class CameraModeSwitchingController : ICameraModeSwitcher
    {
        private readonly ICameraDirector _cameraDirector;

        public CameraModeSwitchingController(ICameraDirector cameraDirector)
        {
            _cameraDirector = cameraDirector;
        }

        public void SwitchNext()
        {
            CameraModeId currentModeId = _cameraDirector.CurrentModeId;
            CameraModeId nextModeId = currentModeId == CameraModeId.Fpp ? CameraModeId.Tpp : CameraModeId.Fpp;
            _cameraDirector.SetBaseMode(nextModeId, CameraTransitionRequest.Default);
        }
    }
}