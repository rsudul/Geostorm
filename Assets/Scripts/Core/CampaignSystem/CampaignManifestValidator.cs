using System.Collections.Generic;

namespace Geostorm.Core.CampaignSystem
{
    public sealed class CampaignManifestValidator
    {
        public CampaignManifestValidationResult Validate(CampaignManifest manifest)
        {
            CampaignManifestValidationResult result = new();

            if (manifest == null)
            {
                result.AddError("Campaign manifest is null.");
                return result;
            }

            ValidateCampaignId(manifest, result);
            ValidateInitialStep(manifest, result);
            ValidateSteps(manifest, result);
            ValidateTransitions(manifest, result);

            return result;
        }

        private static void ValidateCampaignId(CampaignManifest manifest, CampaignManifestValidationResult result)
        {
            if (!manifest.CampaignId.IsValid)
            {
                result.AddError("Campaign id is empty.");
            }
        }

        private static void ValidateInitialStep(CampaignManifest manifest, CampaignManifestValidationResult result)
        {
            if (!manifest.InitialStepId.IsValid)
            {
                result.AddError("Initial step id is empty.");
                return;
            }

            if (!manifest.TryGetStep(manifest.InitialStepId, out _))
            {
                result.AddError($"Initial step '{manifest.InitialStepId}' does not exist in campaign '{manifest.CampaignId}'.");
            }
        }

        private static void ValidateSteps(CampaignManifest manifest, CampaignManifestValidationResult result)
        {
            HashSet<CampaignStepId> stepIds = new();

            for (int i = 0; i < manifest.Steps.Count; i++)
            {
                CampaignStepDefinition step = manifest.Steps[i];
                if (step == null)
                {
                    result.AddError($"Step at index {i} is null.");
                    continue;
                }
                if (!step.Id.IsValid)
                {
                    result.AddError($"Step at index {i} has empty id.");
                    continue;
                }
                if (!stepIds.Add(step.Id))
                {
                    result.AddError($"Duplicated step id '{step.Id}'.");
                }
                if (!step.StepType.IsValid)
                {
                    result.AddError($"Step '{step.Id}' has empty step type.");
                }
                if (step.Config != null && step.Config.StepType != step.StepType)
                {
                    result.AddError($"Step '{step.Id}' has config for step type '{step.Config.StepType}' but step type is '{step.StepType}'.");
                }
                if (step.StepType == CampaignStepType.Scene && string.IsNullOrWhiteSpace(step.SceneName))
                {
                    result.AddError($"Scene step '{step.Id}' has empty scene name.");
                }
            }
        }

        private static void ValidateTransitions(CampaignManifest manifest, CampaignManifestValidationResult result)
        {
            HashSet<CampaignTransitionId> transitionIds = new();
            for (int i = 0; i < manifest.Transitions.Count; i++)
            {
                CampaignTransitionDefinition transition = manifest.Transitions[i];
                if (transition == null)
                {
                    result.AddError($"Transition at index {i} is null.");
                    continue;
                }
                if (!transition.Id.IsValid)
                {
                    result.AddError($"Transition at index {i} has empty id.");
                }
                else if (!transitionIds.Add(transition.Id))
                {
                    result.AddError($"Duplicated transition id '{transition.Id}'.");
                }
                if (!transition.FromStepId.IsValid)
                {
                    result.AddError($"Transition at index {i} has empty from step id.");
                }
                else if (!manifest.TryGetStep(transition.FromStepId, out _))
                {
                    result.AddError($"Transition '{transition.Id}' points from missing step '{transition.FromStepId}'.");
                }
                if (!transition.ToStepId.IsValid)
                {
                    result.AddError($"Transition at index {i} has empty to step id.");
                }
                else if (!manifest.TryGetStep(transition.ToStepId, out _))
                {
                    result.AddError($"Transition '{transition.Id}' points to missing step '{transition.ToStepId}'.");
                }
            }
        }
    }
}
