using System;
using System.Collections.Generic;
using UnityEngine;

namespace Geostorm.Core.CampaignSystem
{
    [CreateAssetMenu(fileName = "CampaignManifest", menuName = "Geostorm/Campaign/Campaign Manifest")]
    public sealed class CampaignManifest : ScriptableObject
    {
        private Dictionary<CampaignStepId, CampaignStepDefinition> _stepsById;

        [SerializeField]
        private string _campaignId = string.Empty;
        [SerializeField]
        private string _initialStepId = string.Empty;
        [SerializeField]
        private List<CampaignStepDefinition> _steps = new();
        [SerializeField]
        private List<CampaignTransitionDefinition> _transitions = new();

        public CampaignId CampaignId => new(_campaignId);
        public CampaignStepId InitialStepId => new(_initialStepId);
        public IReadOnlyList<CampaignStepDefinition> Steps => _steps;
        public IReadOnlyList<CampaignTransitionDefinition> Transitions => _transitions;

        public bool TryGetStep(CampaignStepId stepId, out CampaignStepDefinition step)
        {
            EnsureLookup();
            return _stepsById.TryGetValue(stepId, out step);
        }

        public CampaignStepDefinition GetStep(CampaignStepId stepId)
        {
            if (TryGetStep(stepId, out CampaignStepDefinition step))
            {
                return step;
            }
            throw new InvalidOperationException($"Campaign step '{stepId}' was not found in campaign '{CampaignId}'.");
        }

        public IEnumerable<CampaignTransitionDefinition> GetTransitionFrom(CampaignStepId stepId)
        {
            for (int i = 0; i < _transitions.Count; i++)
            {
                CampaignTransitionDefinition transition = _transitions[i];
                if (transition != null && transition.FromStepId == stepId)
                {
                    yield return transition;
                }
            }
        }

        private void EnsureLookup()
        {
            if (_stepsById != null)
            {
                return;
            }

            _stepsById = new Dictionary<CampaignStepId, CampaignStepDefinition>();

            for (int i = 0; i < _steps.Count; i++)
            {
                CampaignStepDefinition step = _steps[i];
                if (step == null || !step.Id.IsValid)
                {
                    continue;
                }
                _stepsById[step.Id] = step;
            }
        }

        private void OnValidate()
        {
            _stepsById = null;
        }
    }
}
