namespace Geostorm.Core.CampaignSystem
{
    public interface ICampaignStepExecutorRegistry
    {
        bool TryGetExecutor(CampaignStepType stepType, out ICampaignStepExecutor executor);
    }
}
