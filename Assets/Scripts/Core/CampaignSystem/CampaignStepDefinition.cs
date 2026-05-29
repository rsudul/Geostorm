using System;
using UnityEngine;

namespace Geostorm.Core.CampaignSystem
{
    [Serializable]
    public sealed class CampaignStepDefinition
    {
        [SerializeField]
        private string _id = string.Empty;
        [SerializeField]
        private string _displayName = string.Empty;
        [SerializeField]
        private string _stepType = string.Empty;
        [SerializeField]
        private string _sceneName = string.Empty;
        [SerializeField]
        private string _gameplayModeId = string.Empty;
        [SerializeField]
        private CampaignStepConfig _config;

        public CampaignStepId Id => new(_id);
        public string DisplayName => _displayName;
        public CampaignStepType StepType => new(_stepType);
        public string SceneName => _sceneName;
        public GameplayModeId GameplayModeId => new(_gameplayModeId);
        public CampaignStepConfig Config => _config;
    }
}
