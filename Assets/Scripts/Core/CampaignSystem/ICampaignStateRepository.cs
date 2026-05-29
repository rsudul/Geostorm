namespace Geostorm.Core.CampaignSystem
{
    public interface ICampaignStateRepository
    {
        CampaignState GetOrCreateState(CampaignManifest manifest);
        void Save(CampaignState state);
    }
}
