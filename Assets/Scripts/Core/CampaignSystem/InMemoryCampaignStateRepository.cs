using System;
using System.Collections.Generic;

namespace Geostorm.Core.CampaignSystem
{
    public sealed class InMemoryCampaignStateRepository : ICampaignStateRepository
    {
        private readonly Dictionary<CampaignId, CampaignState> _states = new();

        public CampaignState GetOrCreateState(CampaignManifest manifest)
        {
            if (manifest == null)
            {
                throw new ArgumentNullException(nameof(manifest));
            }

            CampaignId campaignId = manifest.CampaignId;
            if (_states.TryGetValue(campaignId, out CampaignState state))
            {
                return state;
            }

            state = new CampaignState(campaignId, manifest.InitialStepId);
            _states[campaignId] = state;
            return state;
        }

        public void Save(CampaignState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }
            _states[state.CampaignId] = state;
        }
    }
}
