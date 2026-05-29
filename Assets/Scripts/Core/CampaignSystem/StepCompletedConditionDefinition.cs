using System;
using UnityEngine;

namespace Geostorm.Core.CampaignSystem
{
    [Serializable]
    public sealed class StepCompletedConditionDefinition : CampaignConditionDefinition
    {
        [SerializeField]
        private string _stepId = string.Empty;

        public CampaignStepId StepId => new(_stepId);
    }
}
