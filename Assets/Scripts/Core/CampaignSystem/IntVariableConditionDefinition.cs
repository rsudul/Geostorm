using System;
using UnityEngine;

namespace Geostorm.Core.CampaignSystem
{
    [Serializable]
    public sealed class IntVariableConditionDefinition : CampaignConditionDefinition
    {
        [SerializeField]
        private string _variableId = string.Empty;
        [SerializeField]
        private IntVariableComparisonOperator _comparisonOperator;
        [SerializeField]
        private int _value;

        public string VariableId => _variableId;
        public IntVariableComparisonOperator ComparisonOperator => _comparisonOperator;
        public int Value => _value;
    }
}
