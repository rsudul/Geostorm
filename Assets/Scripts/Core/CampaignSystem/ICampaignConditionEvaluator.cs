namespace Geostorm.Core.CampaignSystem
{
    public interface ICampaignConditionEvaluator
    {
        bool IsSatisfied(CampaignConditionDefinition condition, CampaignState state);
    }
}
