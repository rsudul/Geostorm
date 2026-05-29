using System;
using System.Collections.Generic;

namespace Geostorm.Core.CampaignSystem
{
    public sealed class CampaignStepExecutorRegistry : ICampaignStepExecutorRegistry, ICampaignStepExecutorRegistryWriter
    {
        private readonly Dictionary<CampaignStepType, ICampaignStepExecutor> _executors = new();

        public bool TryGetExecutor(CampaignStepType stepType, out ICampaignStepExecutor executor)
        {
            if (!stepType.IsValid)
            {
                executor = null;
                return false;
            }
            return _executors.TryGetValue(stepType, out executor);
        }

        public void Register(ICampaignStepExecutor executor)
        {
            if (executor == null)
            {
                throw new ArgumentNullException(nameof(executor));
            }

            if (!executor.StepType.IsValid)
            {
                throw new ArgumentException("Campaign step executor has invalid step type.", nameof(executor));
            }

            _executors[executor.StepType] = executor;
        }

        public void Unregister(ICampaignStepExecutor executor)
        {
            if (executor == null)
            {
                return;
            }

            if (_executors.TryGetValue(executor.StepType, out ICampaignStepExecutor registeredExecutor)
                && ReferenceEquals(registeredExecutor, executor))
            {
                _executors.Remove(executor.StepType);
            }
        }
    }
}
