namespace Geostorm.Core.CampaignSystem
{
    public interface ICampaignStepExecutorRegistryWriter
    {
        void Register(ICampaignStepExecutor executor);
        void Unregister(ICampaignStepExecutor executor);
    }
}
