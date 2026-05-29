using System;
using System.Collections.Generic;

namespace Geostorm.Core.CampaignSystem
{
    public sealed class CampaignProgressionService : ICampaignProgressionService
    {
        private readonly ICampaignConditionEvaluator _conditionEvaluator;

        public CampaignProgressionService(ICampaignConditionEvaluator conditionEvaluator)
        {
            _conditionEvaluator = conditionEvaluator ?? throw new ArgumentNullException(nameof(conditionEvaluator));
        }

        public IReadOnlyList<CampaignTransitionDefinition> GetAvailableTransitions(CampaignManifest manifest, CampaignState state, CampaignStepId stepId)
        {
            if (manifest == null)
            {
                throw new ArgumentNullException(nameof(manifest));
            }

            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            List<CampaignTransitionDefinition> availableTransitions = new();
            foreach (CampaignTransitionDefinition transition in manifest.GetTransitionFrom(stepId))
            {
                if (AreConditionsSatisfied(transition, state))
                {
                    availableTransitions.Add(transition);
                }
            }
            availableTransitions.Sort((left, right) => right.Priority.CompareTo(left.Priority));
            return availableTransitions;
        }

        public bool TrySelectNextTransition(CampaignManifest manifest, CampaignState state, CampaignStepId stepId, out CampaignTransitionDefinition transition)
        {
            IReadOnlyList<CampaignTransitionDefinition> avialableTransitions = GetAvailableTransitions(manifest, state, stepId);
            if (avialableTransitions.Count == 0)
            {
                transition = null;
                return false;
            }
            transition = avialableTransitions[0];
            return true;
        }

        public bool TryGetAvailableTransition(CampaignManifest manifest, CampaignState state, CampaignStepId stepId, CampaignTransitionId transitionId, out CampaignTransitionDefinition transition)
        {
            IReadOnlyList<CampaignTransitionDefinition> availableTransitions = GetAvailableTransitions(manifest, state, stepId);
            for (int i = 0; i < availableTransitions.Count; i++)
            {
                CampaignTransitionDefinition candidate = availableTransitions[i];
                if (candidate.Id == transitionId)
                {
                    transition = candidate;
                    return true;
                }
            }
            transition = null;
            return false;
        }

        private bool AreConditionsSatisfied(CampaignTransitionDefinition transition, CampaignState state)
        {
            IReadOnlyList<CampaignConditionDefinition> conditions = transition.Conditions;
            for (int i = 0; i < conditions.Count; i++)
            {
                if (!_conditionEvaluator.IsSatisfied(conditions[i], state))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
