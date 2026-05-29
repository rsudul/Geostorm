using UnityEngine;

namespace Geostorm.Core.CampaignSystem
{
    public abstract class CampaignStepConfig : ScriptableObject
    {
        public abstract CampaignStepType StepType { get; }
    }
}
