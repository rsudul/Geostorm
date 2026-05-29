namespace Geostorm.Core.CampaignSystem
{
    public interface ICampaignStepContextProvider
    {
        CampaignStepStartContext CurrentContext { get; }
        bool HasCurrentContext { get; }
        bool TryGetCurrentContext(out CampaignStepStartContext context);
    }
}
