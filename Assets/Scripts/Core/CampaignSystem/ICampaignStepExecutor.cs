using System.Threading;
using System.Threading.Tasks;

namespace Geostorm.Core.CampaignSystem
{
    public interface ICampaignStepExecutor
    {
        CampaignStepType StepType { get; }
        Task StartStepAsync(CampaignStepStartContext context, CancellationToken cancellationToken = default);
    }
}
