using System;

namespace Geostorm.Core.CampaignSystem
{
    public sealed class CampaignStateService : ICampaignStateReader, ICampaignStateWriter
    {
        private readonly ICampaignSession _campaignSession;
        private readonly ICampaignStateRepository _stateRepository;

        public CampaignState CurrentState => GetActiveState();
        public bool HasActiveCampaign => _campaignSession.HasActiveCampaign;

        public CampaignStateService(ICampaignSession campaignSession, ICampaignStateRepository stateRepository)
        {
            _campaignSession = campaignSession ?? throw new ArgumentNullException(nameof(campaignSession));
            _stateRepository = stateRepository ?? throw new ArgumentNullException(nameof(stateRepository));
        }

        public bool IsStepCompleted(CampaignStepId stepId)
        {
            return GetActiveState().IsStepCompleted(stepId);
        }

        public bool IsStepUnlocked(CampaignStepId stepId)
        {
            return GetActiveState().IsStepUnlocked(stepId);
        }

        public bool GetFlag(string flagId)
        {
            return GetActiveState().GetFlag(flagId);
        }

        public bool TryGetFlag(string flagId, out bool value)
        {
            return GetActiveState().TryGetFlag(flagId, out value);
        }

        public int GetIntVariable(string variableId)
        {
            return GetActiveState().GetIntVariable(variableId);
        }

        public bool TryGetIntVariable(string variableId, out int value)
        {
            return GetActiveState().TryGetIntVariable(variableId, out value);
        }

        public void SetFlag(string flagId, bool value)
        {
            CampaignState state = GetActiveState();
            state.SetFlag(flagId, value);
            _stateRepository.Save(state);
        }

        public void ClearFlag(string flagId)
        {
            CampaignState state = GetActiveState();
            state.ClearFlag(flagId);
            _stateRepository.Save(state);
        }

        public void SetIntVariable(string variableId, int value)
        {
            CampaignState state = GetActiveState();
            state.SetIntVariable(variableId, value);
            _stateRepository.Save(state);
        }

        public void AddIntVariable(string variableId, int delta)
        {
            CampaignState state = GetActiveState();
            state.AddIntVariable(variableId, delta);
            _stateRepository.Save(state);
        }

        private CampaignState GetActiveState()
        {
            if (!_campaignSession.HasActiveCampaign || _campaignSession.CurrentState == null)
            {
                throw new InvalidOperationException("Cannot access campaign state because no campaign is active.");
            }
            return _campaignSession.CurrentState;
        }
    }
}
