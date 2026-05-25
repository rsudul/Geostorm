using System;

namespace Geostorm.Core.CharacterSystem
{
    public sealed class PossessionEventArgs : EventArgs
    {
        public int PlayerId { get; }
        public IPawn PossessedPawn { get; }
        public ICharacterBrain AssignedBrain { get; }

        public PossessionEventArgs(int playerId, IPawn possessedPawn, ICharacterBrain assignedBrain)
        {
            PlayerId = playerId;
            PossessedPawn = possessedPawn;
            AssignedBrain = assignedBrain;
        }
    }
}