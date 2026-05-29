using System;
using System.Collections.Generic;

namespace Geostorm.Core.CampaignSystem
{
    public sealed class CampaignState
    {
        private readonly HashSet<CampaignStepId> _completedStepIds = new();
        private readonly HashSet<CampaignStepId> _unlockedStepIds = new();
        private readonly Dictionary<string, bool> _flags = new(StringComparer.Ordinal);
        private readonly Dictionary<string, int> _intVariables = new(StringComparer.Ordinal);

        public CampaignId CampaignId { get; }
        public CampaignStepId CurrentStepId { get; private set; }
        public IReadOnlyCollection<CampaignStepId> CompletedStepIds => _completedStepIds;
        public IReadOnlyCollection<CampaignStepId> UnlockedStepIds => _unlockedStepIds;

        public CampaignState(CampaignId campaignId, CampaignStepId currentStepId)
        {
            CampaignId = campaignId;
            CurrentStepId = currentStepId;
            if (currentStepId.IsValid)
            {
                _unlockedStepIds.Add(currentStepId);
            }
        }

        internal void SetCurrentStep(CampaignStepId stepId)
        {
            CurrentStepId = stepId;
            UnlockStep(stepId);
        }

        internal void CompleteStep(CampaignStepId stepId)
        {
            if (stepId.IsValid)
            {
                _completedStepIds.Add(stepId);
            }
        }

        public bool IsStepCompleted(CampaignStepId stepId)
        {
            return stepId.IsValid && _completedStepIds.Contains(stepId);
        }

        internal void UnlockStep(CampaignStepId stepId)
        {
            if (stepId.IsValid)
            {
                _unlockedStepIds.Add(stepId);
            }
        }

        public bool IsStepUnlocked(CampaignStepId stepId)
        {
            return stepId.IsValid && _unlockedStepIds.Contains(stepId);
        }

        internal void SetFlag(string flagId, bool value)
        {
            if (string.IsNullOrEmpty(flagId))
            {
                return;
            }
            _flags[flagId] = value;
        }

        internal void ClearFlag(string flagId)
        {
            if (string.IsNullOrWhiteSpace(flagId))
            {
                return;
            }
            _flags.Remove(flagId);
        }

        public bool GetFlag(string flagId)
        {
            return !string.IsNullOrEmpty(flagId) && _flags.TryGetValue(flagId, out bool value) && value;
        }

        public bool TryGetFlag(string flagId, out bool value)
        {
            if (string.IsNullOrWhiteSpace(flagId))
            {
                value = false;
                return false;
            }
            return _flags.TryGetValue(flagId, out value);
        }

        internal void SetIntVariable(string variableId, int value)
        {
            if (string.IsNullOrEmpty(variableId))
            {
                return;
            }
            _intVariables[variableId] = value;
        }

        internal void AddIntVariable(string variableId, int delta)
        {
            if (string.IsNullOrWhiteSpace(variableId))
            {
                return;
            }
            int currentValue = GetIntVariable(variableId);
            _intVariables[variableId] = currentValue + delta;
        }

        public int GetIntVariable(string variableId)
        {
            if (string.IsNullOrEmpty(variableId))
            {
                return 0;
            }
            return _intVariables.TryGetValue(variableId, out int value) ? value : 0;
        }

        public bool TryGetIntVariable(string variableId, out int value)
        {
            if (string.IsNullOrWhiteSpace(variableId))
            {
                value = 0;
                return false;
            }
            return _intVariables.TryGetValue(variableId, out value);
        }
    }
}
