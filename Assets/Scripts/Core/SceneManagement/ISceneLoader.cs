using System.Threading;
using System.Threading.Tasks;

namespace Geostorm.Core.SceneManagement
{
    public interface ISceneLoader
    {
        Task LoadSceneAsync(string sceneName, object payload = null, CancellationToken cancellationToken = default);
        T ConsumePayload<T>(string sceneName) where T : class;
    }
}