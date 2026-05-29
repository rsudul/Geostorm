namespace Geostorm.Core.CampaignSystem
{
    public interface ICampaignStateWriter
    {
        void SetFlag(string flagId, bool value);
        void ClearFlag(string flagId);
        void SetIntVariable(string variableId, int value);
        void AddIntVariable(string variableId, int delta);
    }
}
