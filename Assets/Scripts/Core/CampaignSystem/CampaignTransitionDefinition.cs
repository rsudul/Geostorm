using System;
using System.Collections.Generic;
using UnityEngine;

namespace Geostorm.Core.CampaignSystem
{
    [Serializable]
    public sealed class CampaignTransitionDefinition
    {
        [SerializeField]
        private string _id = string.Empty;
        [SerializeField]
        private string _fromStepId = string.Empty;
        [SerializeField]
        private string _toStepId = string.Empty;
        [SerializeField]
        private int _priority;
        [SerializeReference]
        private List<CampaignConditionDefinition> _conditions = new();

        public CampaignTransitionId Id => new(_id);
        public CampaignStepId FromStepId => new(_fromStepId);
        public CampaignStepId ToStepId => new(_toStepId);
        public int Priority => _priority;
        public IReadOnlyList<CampaignConditionDefinition> Conditions => _conditions;
    }
}
