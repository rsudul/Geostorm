namespace Geostorm.Core.CampaignSystem
{
    public interface ICampaignStateReader
    {
        CampaignState CurrentState { get; }
        bool HasActiveCampaign { get; }
        bool IsStepCompleted(CampaignStepId stepId);
        bool IsStepUnlocked(CampaignStepId stepId);
        bool GetFlag(string flagId);
        bool TryGetFlag(string flagId, out bool value);
        int GetIntVariable(string variableId);
        bool TryGetIntVariable(string variableId, out int value);
    }
}
