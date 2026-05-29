using UnityEngine;

namespace Geostorm.Core.CampaignSystem
{
    [CreateAssetMenu(fileName = "SceneCampaignStepConfig", menuName = "Geostorm/Campaign/Step Configs/Scene Step Config")]
    public sealed class SceneCampaignStepConfig : CampaignStepConfig
    {
        [SerializeField]
        private bool _allowPlayerInput = true;
        [SerializeField]
        private bool _autoCompleteOnSceneLoaded;

        public override CampaignStepType StepType => CampaignStepType.Scene;

        public bool AllowPlayerInput => _allowPlayerInput;
        public bool AutoCompleteOnSceneLoaded => _autoCompleteOnSceneLoaded;
    }
}
