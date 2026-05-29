using System;
using System.Threading;
using System.Threading.Tasks;

namespace Geostorm.Core.CampaignSystem
{
    public sealed class CampaignSession : ICampaignSession
    {
        private readonly ICampaignRunner _campaignRunner;
        private readonly CampaignManifestValidator _manifestValidator;

        public CampaignManifest CurrentManifest { get; private set; }
        public CampaignState CurrentState => _campaignRunner.CurrentState;
        public bool HasActiveCampaign => CurrentManifest != null;

        public CampaignSession(ICampaignRunner campaignRunner, CampaignManifestValidator manifestValidator)
        {
            _campaignRunner = campaignRunner ?? throw new ArgumentNullException(nameof(campaignRunner));
            _manifestValidator = manifestValidator ?? throw new ArgumentNullException(nameof(manifestValidator));
        }

        public async Task StartCampaignAsync(CampaignManifest manifest, CancellationToken cancellationToken = default)
        {
            if (manifest == null)
            {
                throw new ArgumentNullException(nameof(manifest));
            }

            CampaignManifestValidationResult validationResult = _manifestValidator.Validate(manifest);
            if (!validationResult.IsValid)
            {
                throw new InvalidOperationException(BuildValidationErrorMessage(manifest, validationResult));
            }

            CurrentManifest = manifest;
            await _campaignRunner.StartCampaignAsync(manifest, cancellationToken);
        }

        public async Task StartStepAsync(CampaignStepId stepId, CampaignTransitionDefinition selectedTransition = null, CancellationToken cancellationToken = default)
        {
            if (CurrentManifest == null)
            {
                throw new InvalidOperationException("Cannot start campaign step because no campaign is active.");
            }
            await _campaignRunner.StartStepAsync(CurrentManifest, stepId, selectedTransition, cancellationToken);
        }

        public async Task<bool> CompleteCurrentStepAndAdvanceAsync(CancellationToken cancellationToken = default)
        {
            if (CurrentManifest == null)
            {
                throw new InvalidOperationException("Cannot complete campaign step because no campaign is active.");
            }
            return await _campaignRunner.CompleteCurrentStepAndAdvanceAsync(CurrentManifest, cancellationToken);
        }

        public async Task<bool> CompleteCurrentStepWithTransitionAsync(CampaignTransitionId transitionId, CancellationToken cancellationToken = default)
        {
            if (CurrentManifest == null)
            {
                throw new InvalidOperationException("Cannot complete campaign step because no campaign is active.");
            }
            return await _campaignRunner.CompleteCurrentStepWithTransitionAsync(CurrentManifest, transitionId, cancellationToken);
        }

        private static string BuildValidationErrorMessage(CampaignManifest manifest, CampaignManifestValidationResult validationResult)
        {
            string campaignName = manifest != null ? manifest.name : "null";
            return $"Campaign manifest '{campaignName}' is invalid:\n- " + string.Join("\n- ", validationResult.Errors);
        }
    }
}
