using System;
using VContainer.Unity;
using Geostorm.Core.CharacterSystem;
using Geostorm.Core.Input;
using Geostorm.Infrastructure.CharacterSystem;
using Geostorm.Infrastructure.Input;

namespace Geostorm.Infrastructure
{
    public class TestLevelInitializer : IStartable, IDisposable
    {
        private readonly PossessionManager _possesionManager;
        private readonly PlayerBrain _playerBrain;
        private readonly Func<AIBrain> _aiBrainFactory;
        private readonly IInputProvider _inputProvider;
        private readonly InputRouter _inputRouter;
        private int _currentPlayerPawnIndex = 0;

        public TestLevelInitializer(PossessionManager pm, PlayerBrain pBrain, Func<AIBrain> aiBrainFactory, IInputProvider inputProvider, InputRouter inputRouter)
        {
            _possesionManager = pm;
            _playerBrain = pBrain;
            _aiBrainFactory = aiBrainFactory;
            _inputProvider = inputProvider;
            _inputRouter = inputRouter;
        }

        public void Start()
        {
            var pawns = _possesionManager.AvailablePawns;
            if (pawns.Count == 0)
            {
                return;
            }

            IPawn playerPawn = pawns[0];
            for (int i = 0; i < pawns.Count; i++)
            {
                if (i == 0)
                {
                    _possesionManager.PossessAsPlayer(0, pawns[i], _playerBrain);
                    _currentPlayerPawnIndex = 0;
                }
                else
                {
                    _possesionManager.AssignBrain(pawns[i], _aiBrainFactory(), playerPawn);
                }
            }

            _inputRouter.PushContext(new GameplayInputContext());
            _inputProvider.OnNextCharacter += HandleNextCharacter;
        }

        private void HandleNextCharacter(object sender, NextCharacterEventArgs args)
        {
            var pawns = _possesionManager.AvailablePawns;
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
            _possesionManager.AssignBrain(currentPawn, _aiBrainFactory(), nextPawn);
            _possesionManager.PossessAsPlayer(0, nextPawn, _playerBrain);
        }

        public void Dispose()
        {
            if (_inputProvider != null)
            {
                _inputProvider.OnNextCharacter -= HandleNextCharacter;
            }
        }
    }
}