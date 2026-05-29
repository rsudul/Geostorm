using System.Collections.Generic;

namespace Geostorm.Core.CampaignSystem
{
    public interface ICampaignProgressionService
    {
        IReadOnlyList<CampaignTransitionDefinition> GetAvailableTransitions(CampaignManifest manifest, CampaignState state, CampaignStepId stepId);
        bool TrySelectNextTransition(CampaignManifest manifest, CampaignState state, CampaignStepId stepId, out CampaignTransitionDefinition transition);
        bool TryGetAvailableTransition(CampaignManifest manifest, CampaignState state, CampaignStepId stepId, CampaignTransitionId transitionId, out CampaignTransitionDefinition transition);
    }
}
