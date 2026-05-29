using System.Threading;
using System.Threading.Tasks;
using Geostorm.Core.CampaignSystem;
using Geostorm.Core.SceneManagement;


namespace Geostorm.Infrastructure.CampaignSystem
{
    public sealed class SceneCampaignStepExecutor : ICampaignStepExecutor
    {
        private readonly ISceneLoader _sceneLoader;

        public CampaignStepType StepType => CampaignStepType.Scene;

        public SceneCampaignStepExecutor(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public Task StartStepAsync(CampaignStepStartContext context, CancellationToken cancellationToken = default)
        {
            return _sceneLoader.LoadSceneAsync(context.Step.SceneName, context, cancellationToken);
        }
    }
}
