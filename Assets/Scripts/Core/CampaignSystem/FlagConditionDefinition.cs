using System;
using UnityEngine;

namespace Geostorm.Core.CampaignSystem
{
    [Serializable]
    public sealed class FlagConditionDefinition : CampaignConditionDefinition
    {
        [SerializeField]
        private string _flagId = string.Empty;
        [SerializeField]
        private bool _expectedValue = true;

        public string FlagId => _flagId;
        public bool ExpectedValue => _expectedValue;
    }
}
