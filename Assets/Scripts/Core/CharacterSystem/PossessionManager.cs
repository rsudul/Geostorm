using System;
using System.Collections.Generic;

namespace Geostorm.Core.CharacterSystem
{
    public sealed class PossessionManager
    {
        private readonly List<IPawn> _availablePawns = new();

        public IReadOnlyList<IPawn> AvailablePawns => _availablePawns;

        public event EventHandler<PossessionEventArgs> OnPossessionChanged;

        public void RegisterPawn(IPawn pawn)
        {
            if (!_availablePawns.Contains(pawn))
            {
                _availablePawns.Add(pawn);
            }
        }

        public void UnregisterPawn(IPawn pawn)
        {
            if (_availablePawns.Contains(pawn))
            {
                _availablePawns.Remove(pawn);
            }
        }

        public void AssignBrain(IPawn pawn, ICharacterBrain newBrain, object context = null)
        {
            if (pawn.CurrentBrain != null)
            {
                pawn.CurrentBrain.OnUnpossess();
            }

            pawn.SetBrain(newBrain);

            if (newBrain != null)
            {
                newBrain.OnPossess(pawn, context);
            }
        }

        public void PossessAsPlayer(int playerId, IPawn targetPawn, ICharacterBrain playerBrain, object context = null)
        {
            if (targetPawn != null && playerBrain != null)
            {
                AssignBrain(targetPawn, playerBrain, context);
                OnPossessionChanged?.Invoke(this, new PossessionEventArgs(playerId, targetPawn, playerBrain));
            }
        }
    }
}