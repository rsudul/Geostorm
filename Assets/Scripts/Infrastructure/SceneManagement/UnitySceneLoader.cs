using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Geostorm.Core.SceneManagement;
using UnityEngine;
using System.Collections.Concurrent;

namespace Geostorm.Infrastructure.SceneManagement
{
    public sealed class UnitySceneLoader : ISceneLoader
    {
        private const float LoadingTimeoutSeconds = 300.0f;
        private readonly string _loadingSceneName;
        private readonly ConcurrentDictionary<string, object> _scenePayloads = new();

        public UnitySceneLoader(SceneConfiguration config)
        {
            _loadingSceneName = config.LoadingSceneName;
        }

        public async Task LoadSceneAsync(string sceneName, object payload = null, CancellationToken cancellationToken = default)
        {
            if (payload != null)
            {
                _scenePayloads[sceneName] = payload;
            }

            if (Application.CanStreamedLevelBeLoaded(_loadingSceneName))
            {
                await LoadOperationAsync(SceneManager.LoadSceneAsync(_loadingSceneName), _loadingSceneName, cancellationToken);
                var unloadOp = Resources.UnloadUnusedAssets();
                while (!unloadOp.isDone)
                {
                    await Task.Yield();
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            else
            {
                Debug.LogWarning($"No scene with name '{_loadingSceneName}' found.");
            }

            var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            if (asyncOperation == null)
            {
                throw new Exception($"Could not load scene: {sceneName}");
            }

            asyncOperation.allowSceneActivation = false;
            await LoadOperationAsync(asyncOperation, sceneName, cancellationToken);
            asyncOperation.allowSceneActivation = true;
        }

        public T ConsumePayload<T>(string sceneName) where T : class
        {
            if (_scenePayloads.TryRemove(sceneName, out object payload))
            {
                return payload as T;
            }
            return null;
        }

        private async Task LoadOperationAsync(AsyncOperation asyncOperation, string targetScene, CancellationToken cancellationToken)
        {
            float timer = 0.0f;

            while (!asyncOperation.isDone)
            {
                cancellationToken.ThrowIfCancellationRequested();
                timer += Time.unscaledDeltaTime;
                if (timer > LoadingTimeoutSeconds)
                {
                    throw new TimeoutException($"Loading scene time exceeded: {targetScene}");
                }

                if (!asyncOperation.allowSceneActivation && asyncOperation.progress >= 0.9f)
                {
                    break;
                }

                await Task.Yield();
            }
        }
    }
}