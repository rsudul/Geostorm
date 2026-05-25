using VContainer.Unity;
using Geostorm.Core.SceneManagement;

namespace Geostorm.Infrastructure
{
    public sealed class Bootstrapper : IStartable
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly SceneConfiguration _config;

        public Bootstrapper(ISceneLoader sceneLoader, SceneConfiguration config)
        {
            _sceneLoader = sceneLoader;
            _config = config;
        }

        public void Start()
        {
            _ = _sceneLoader.LoadSceneAsync(_config.InitialSceneName);
        }
    }
}