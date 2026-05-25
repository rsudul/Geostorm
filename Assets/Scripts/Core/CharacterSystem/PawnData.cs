using UnityEngine;

namespace Geostorm.Core.CharacterSystem
{
    [CreateAssetMenu(fileName = "PawnData", menuName = "Geostorm/Pawn Data")]
    public sealed class PawnData : ScriptableObject
    {
        [SerializeField]
        private float _moveSpeed = 5.0f;
        [SerializeField]
        private float _gravity = -9.81f;

        public float MoveSpeed => _moveSpeed;
        public float Gravity => _gravity;
    }
}