using System;
using UnityEngine;
using VContainer.Unity;
using Geostorm.Core.CameraSystem;
using Geostorm.Core.CharacterSystem;

namespace Geostorm.Infrastructure.CameraSystem
{
    public sealed class PossessionCameraBinder : IInitializable, IDisposable
    {
        private readonly PossessionManager _possessionManager;
        private readonly ICameraDirector _cameraDirector;

        public PossessionCameraBinder(PossessionManager possessionManager, ICameraDirector cameraDirector)
        {
            _possessionManager = possessionManager;
            _cameraDirector = cameraDirector;
        }

        public void Initialize()
        {
            if (_possessionManager != null)
            {
                _possessionManager.OnPossessionChanged += HandlePossessionChanged;
            }
        }

        public void Dispose()
        {
            if (_possessionManager != null)
            {
                _possessionManager.OnPossessionChanged -= HandlePossessionChanged;
            }
        }

        private void HandlePossessionChanged(object sender, PossessionEventArgs args)
        {
            if (args.PossessedPawn == null)
            {
                return;
            }

            if (!TryGetCameraTargetProvider(args.PossessedPawn, out ICameraTargetProvider targetProvider))
            {
                Debug.LogWarning($"Possessed pawn does not provide camera target: {args.PossessedPawn}");
                return;
            }

            _cameraDirector.SetTargetProvider(targetProvider);
            _cameraDirector.SetBaseMode(CameraModeId.Tpp, CameraTransitionRequest.Default);
        }

        private static bool TryGetCameraTargetProvider(IPawn pawn, out ICameraTargetProvider targetProvider)
        {
            targetProvider = null;

            if (pawn is not Component component)
            {
                return false;
            }

            if (component.TryGetComponent(out PawnCameraTargets pawnCameraTargets))
            {
                targetProvider = pawnCameraTargets;
                return true;
            }

            return component.TryGetComponent(out targetProvider);
        }
    }
}