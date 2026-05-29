using System.Threading;
using System.Threading.Tasks;

namespace Geostorm.Core.CampaignSystem
{
    public interface ICampaignSession
    {
        CampaignManifest CurrentManifest { get; }
        CampaignState CurrentState { get; }
        bool HasActiveCampaign { get; }
        Task StartCampaignAsync(CampaignManifest manifest, CancellationToken cancellationToken = default);
        Task StartStepAsync(CampaignStepId stepId, CampaignTransitionDefinition selectedTransition = null, CancellationToken cancellationToken = default);
        Task<bool> CompleteCurrentStepAndAdvanceAsync(CancellationToken cancellationToken = default);
        Task<bool> CompleteCurrentStepWithTransitionAsync(CampaignTransitionId transitionId, CancellationToken cancellationToken = default);
    }
}
