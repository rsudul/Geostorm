using VContainer.Unity;
using Geostorm.Core.CampaignSystem;

namespace Geostorm.Infrastructure.CampaignSystem
{
    public sealed class CampaignStepExecutorRegistration : IStartable
    {
        private readonly ICampaignStepExecutorRegistryWriter _registry;
        private readonly SceneCampaignStepExecutor _sceneCampaignStepExecutor;

        public CampaignStepExecutorRegistration(ICampaignStepExecutorRegistryWriter registry, SceneCampaignStepExecutor sceneCampaignStepExecutor)
        {
            _registry = registry;
            _sceneCampaignStepExecutor = sceneCampaignStepExecutor;
        }

        public void Start()
        {
            _registry.Register(_sceneCampaignStepExecutor);
        }
    }
}
