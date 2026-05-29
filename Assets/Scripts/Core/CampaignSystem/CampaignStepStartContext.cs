using System;

namespace Geostorm.Core.CampaignSystem
{
    public sealed class CampaignStepStartContext
    {
        public CampaignManifest Manifest { get; }
        public CampaignState State { get; }
        public CampaignStepDefinition Step { get; }
        public CampaignTransitionDefinition SelectedTransition { get; }

        public CampaignStepStartContext(CampaignManifest manifest, CampaignState state, CampaignStepDefinition step, CampaignTransitionDefinition selectedTransition = null)
        {
            Manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
            State = state ?? throw new ArgumentNullException(nameof(state));
            Step = step ?? throw new ArgumentNullException(nameof(step));
            SelectedTransition = selectedTransition ?? throw new ArgumentNullException(nameof(selectedTransition));
        }

        public bool TryGetConfig<TConfig>(out TConfig config) where TConfig : CampaignStepConfig
        {
            config = Step.Config as TConfig;
            return config != null;
        }

        public TConfig GetConfig<TConfig>() where TConfig : CampaignStepConfig
        {
            if (Step.Config is TConfig config)
            {
                return config;
            }

            string actualTypeName = Step.Config != null ? Step.Config.GetType().Name : "null";
            throw new InvalidOperationException($"Campaign step '{Step.Id}' expected config of type '{typeof(TConfig).Name}' but got '{actualTypeName}'.");
        }
    }
}
