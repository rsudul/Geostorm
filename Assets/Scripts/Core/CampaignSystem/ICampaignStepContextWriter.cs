namespace Geostorm.Core.CampaignSystem
{
    public interface ICampaignStepContextWriter
    {
        void SetCurrentContext(CampaignStepStartContext context);
        void ClearCurrentContext();
    }
}
