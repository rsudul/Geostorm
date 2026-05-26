using VContainer.Unity;
using Geostorm.Core.CameraSystem;

namespace Geostorm.Infrastructure.CameraSystem
{
    public sealed class CameraSystemUpdater : ILateTickable
    {
        private readonly ICameraDirector _cameraDirector;

        public CameraSystemUpdater(ICameraDirector cameraDirector)
        {
            _cameraDirector = cameraDirector;
        }

        public void LateTick()
        {
            _cameraDirector.Tick(UnityEngine.Time.deltaTime);
        }
    }
}