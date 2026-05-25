using UnityEngine;

namespace Geostorm.Infrastructure.CameraSystem
{
    [CreateAssetMenu(fileName = "TppCameraRigData", menuName = "Geostorm/Camera/Tpp Camera Rig Data")]
    public sealed class TppCameraRigData : ScriptableObject
    {
        [Header("Priority")]
        [SerializeField]
        private int _activePriority = 100;
        [SerializeField]
        private int _inactivePriority = 0;

        [Header("View Frame")]
        [SerializeField]
        private bool _flattenViewFrame = true;

        public int ActivePriority => _activePriority;
        public int InactivePriority => _inactivePriority;
        public bool FlattenViewFrame => _flattenViewFrame;
    }
}