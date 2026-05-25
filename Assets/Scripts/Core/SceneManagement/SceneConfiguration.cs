using UnityEngine;

namespace Geostorm.Core.SceneManagement
{
    [CreateAssetMenu(fileName = "SceneConfiguration", menuName = "Geostorm/Scene Configuration")]
    public sealed class SceneConfiguration : ScriptableObject
    {
        [SerializeField]
        private string _loadingSceneName = "";
        [SerializeField]
        private string _initialSceneName = "";

        public string LoadingSceneName => _loadingSceneName;
        public string InitialSceneName => _initialSceneName;
    }
}