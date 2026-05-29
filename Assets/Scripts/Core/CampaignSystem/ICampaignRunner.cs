using System.Threading;
using System.Threading.Tasks;

namespace Geostorm.Core.CampaignSystem
{
    public interface ICampaignRunner
    {
        CampaignState CurrentState { get; }
        Task StartCampaignAsync(CampaignManifest manifest, CancellationToken cancellationToken = default);
        Task StartStepAsync(CampaignManifest manifest, CampaignStepId stepId, CampaignTransitionDefinition selectedTransition = null, CancellationToken cancellationToken = default);
        Task<bool> CompleteCurrentStepAndAdvanceAsync(CampaignManifest manifest, CancellationToken cancellationToken = default);
        Task<bool> CompleteCurrentStepWithTransitionAsync(CampaignManifest manifest, CampaignTransitionId transitionId, CancellationToken cancellationToken = default);
    }
}
