using VContainer.Unity;
using Geostorm.Core.CameraSystem;

namespace Geostorm.Infrastructure.CameraSystem
{
    public sealed class CameraSystemUpdater : ITickable
    {
        private readonly ICameraDirector _cameraDirector;

        public CameraSystemUpdater(ICameraDirector cameraDirector)
        {
            _cameraDirector = cameraDirector;
        }

        public void Tick()
        {
            _cameraDirector.Tick(UnityEngine.Time.deltaTime);
        }
    }
}