using UnityEngine;

namespace Geostorm.Core.CharacterSystem
{
    public interface IPawn
    {
        ICharacterBrain CurrentBrain { get; }
        Vector3 Position { get; }
        void SetBrain(ICharacterBrain newBrain);
        void Move(Vector3 direction);
    }
}