using System;
using System.Threading;
using System.Threading.Tasks;

namespace Geostorm.Core.CampaignSystem
{
    public sealed class CampaignRunner : ICampaignRunner
    {
        private readonly ICampaignStateRepository _stateRepository;
        private readonly ICampaignProgressionService _progressionService;
        private readonly ICampaignStepExecutorRegistry _executorRegistry;
        private readonly ICampaignStepContextWriter _contextWriter;

        public CampaignState CurrentState { get; private set; }

        public CampaignRunner(
            ICampaignStateRepository stateRepository,
            ICampaignProgressionService progressionService,
            ICampaignStepExecutorRegistry executorRegistry,
            ICampaignStepContextWriter contextWriter
        )
        {
            _stateRepository = stateRepository ?? throw new ArgumentNullException(nameof(stateRepository));
            _progressionService = progressionService ?? throw new ArgumentNullException(nameof(progressionService));
            _executorRegistry = executorRegistry ?? throw new ArgumentNullException(nameof(executorRegistry));
            _contextWriter = contextWriter ?? throw new ArgumentNullException(nameof(contextWriter));
        }

        public async Task StartCampaignAsync(CampaignManifest manifest, CancellationToken cancellationToken = default)
        {
            if (manifest == null)
            {
                throw new ArgumentNullException(nameof(manifest));
            }

            CurrentState = _stateRepository.GetOrCreateState(manifest);
            CampaignStepId stepId = CurrentState.CurrentStepId.IsValid ? CurrentState.CurrentStepId : manifest.InitialStepId;
            await StartStepAsync(manifest, stepId, null, cancellationToken);
        }

        public async Task StartStepAsync(CampaignManifest manifest, CampaignStepId stepId, CampaignTransitionDefinition selectedTransition = null, CancellationToken cancellationToken = default)
        {
            if (manifest == null)
            {
                throw new ArgumentNullException(nameof(manifest));
            }

            CurrentState ??= _stateRepository.GetOrCreateState(manifest);
            CampaignStepDefinition step = manifest.GetStep(stepId);
            ValidateStepConfig(step);
            CurrentState.SetCurrentStep(step.Id);
            _stateRepository.Save(CurrentState);

            if (!_executorRegistry.TryGetExecutor(step.StepType, out ICampaignStepExecutor executor))
            {
                throw new InvalidOperationException($"Campaign step executor for type '{step.StepType}' is not registered.");
            }

            CampaignStepStartContext context = new(manifest, CurrentState, step, selectedTransition);
            _contextWriter.SetCurrentContext(context);
            await executor.StartStepAsync(context, cancellationToken);
        }

        public async Task<bool> CompleteCurrentStepAndAdvanceAsync(CampaignManifest manifest, CancellationToken cancellationToken = default)
        {
            if (manifest == null)
            {
                throw new ArgumentNullException(nameof(manifest));
            }

            CurrentState ??= _stateRepository.GetOrCreateState(manifest);
            CampaignStepId completedStepId = CurrentState.CurrentStepId;
            CurrentState.CompleteStep(completedStepId);

            if (!_progressionService.TrySelectNextTransition(manifest, CurrentState, completedStepId, out CampaignTransitionDefinition transition))
            {
                _stateRepository.Save(CurrentState);
                return false;
            }

            CurrentState.UnlockStep(transition.ToStepId);
            _stateRepository.Save(CurrentState);

            await StartStepAsync(manifest, transition.ToStepId, transition, cancellationToken);
            return true;
        }

        public async Task<bool> CompleteCurrentStepWithTransitionAsync(CampaignManifest manifest, CampaignTransitionId transitionId, CancellationToken cancellationToken = default)
        {
            if (manifest == null)
            {
                throw new ArgumentNullException(nameof(manifest));
            }

            CurrentState ??= _stateRepository.GetOrCreateState(manifest);
            CampaignStepId completedStepId = CurrentState.CurrentStepId;
            CurrentState.CompleteStep(completedStepId);

            if (!_progressionService.TryGetAvailableTransition(manifest, CurrentState, completedStepId, transitionId, out CampaignTransitionDefinition transition))
            {
                _stateRepository.Save(CurrentState);
                return false;
            }

            CurrentState.UnlockStep(transition.ToStepId);
            _stateRepository.Save(CurrentState);

            await StartStepAsync(manifest, transition.ToStepId, transition, cancellationToken);

            return true;
        }

        private static void ValidateStepConfig(CampaignStepDefinition step)
        {
            if (step.Config == null)
            {
                return;
            }

            if (step.Config.StepType != step.StepType)
            {
                throw new InvalidOperationException($"Campaign step '{step.Id}' has config for step type '{step.Config.StepType}' but step type is '{step.StepType}'.");
            }
        }
    }
}
