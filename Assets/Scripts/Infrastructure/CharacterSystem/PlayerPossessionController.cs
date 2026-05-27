using System;
using Geostorm.Core.CharacterSystem;

namespace Geostorm.Infrastructure.CharacterSystem
{
    public sealed class PlayerPossessionController : IPlayerCharacterSwitcher
    {
        private readonly PossessionManager _possessionManager;
        private readonly PlayerBrain _playerBrain;
        private readonly Func<AIBrain> _aiBrainFactory;

        private int _currentPlayerPawnIndex;

        public PlayerPossessionController(
            PossessionManager possessionManager,
            PlayerBrain playerBrain,
            Func<AIBrain> aiBrainFactory
        )
        {
            _possessionManager = possessionManager;
            _playerBrain = playerBrain;
            _aiBrainFactory = aiBrainFactory;
        }

        public void InitializePlayerPossession()
        {
            var pawns = _possessionManager.AvailablePawns;
            if (pawns.Count == 0)
            {
                return;
            }

            IPawn playerPawn = pawns[0];
            for (int i = 0; i < pawns.Count; i++)
            {
                if (i == 0)
                {
                    _possessionManager.PossessAsPlayer(0, pawns[i], _playerBrain);
                    _currentPlayerPawnIndex = 0;
                }
                else
                {
                    _possessionManager.AssignBrain(pawns[i], _aiBrainFactory(), playerPawn);
                }
            }
        }

        public void SwitchToNextCharacter()
        {
            var pawns = _possessionManager.AvailablePawns;
            if (pawns.Count < 2)
            {
                return;
            }

            IPawn currentPawn = pawns[_currentPlayerPawnIndex];
            _currentPlayerPawnIndex++;
            if (_currentPlayerPawnIndex >= pawns.Count)
            {
                _currentPlayerPawnIndex = 0;
            }
            IPawn nextPawn = pawns[_currentPlayerPawnIndex];

            _possessionManager.AssignBrain(currentPawn, _aiBrainFactory(), nextPawn);
            _possessionManager.PossessAsPlayer(0, nextPawn, _playerBrain);
        }
    }
}