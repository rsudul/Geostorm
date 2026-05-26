using UnityEngine;

namespace Geostorm.Core.CharacterSystem
{
    [CreateAssetMenu(fileName = "PawnData", menuName = "Geostorm/Pawn Data")]
    public sealed class PawnData : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField]
        private float _moveSpeed = 5.0f;
        [SerializeField]
        private float _rotationSpeed = 10.0f;

        [Header("Physics")]
        [SerializeField]
        private float _gravity = -9.81f;

        public float MoveSpeed => _moveSpeed;
        public float RotationSpeed => _rotationSpeed;

        public float Gravity => _gravity;
    }
}