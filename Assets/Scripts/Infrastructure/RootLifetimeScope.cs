using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Geostorm.Core.CameraSystem;
using Geostorm.Core.CampaignSystem;
using Geostorm.Core.CharacterSystem;
using Geostorm.Core.Input;
using Geostorm.Core.SceneManagement;
using Geostorm.Infrastructure.CameraSystem;
using Geostorm.Infrastructure.CampaignSystem;
using Geostorm.Infrastructure.Input;
using Geostorm.Infrastructure.SceneManagement;

namespace Geostorm.Infrastructure
{
    public sealed class RootLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private SceneConfiguration _sceneConfiguration;
        [SerializeField]
        private CampaignManifest _campaignManifest;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_sceneConfiguration);
            builder.RegisterInstance(_campaignManifest);

            builder.Register<UnityInputProvider>(Lifetime.Singleton).As<IInputProvider, IDisposable>();
            builder.Register<InputRouter>(Lifetime.Singleton);

            builder.Register<PossessionManager>(Lifetime.Singleton);

            builder.Register<CameraStack>(Lifetime.Singleton);
            builder.Register<CameraRigRegistry>(Lifetime.Singleton);
            builder.Register<CameraDirector>(Lifetime.Singleton).As<ICameraDirector>().As<IViewFrameProvider>().AsSelf();
            builder.Register<CameraLookInputState>(Lifetime.Singleton).AsSelf().As<ICameraLookInput>().As<ICameraLookInputWriter>();
            builder.RegisterEntryPoint<CameraSystemUpdater>();

            builder.Register<UnitySceneLoader>(Lifetime.Singleton).As<ISceneLoader>();

            builder.Register<DefaultCampaignConditionEvaluator>(Lifetime.Singleton).As<ICampaignConditionEvaluator>();
            builder.Register<CampaignProgressionService>(Lifetime.Singleton).As<ICampaignProgressionService>();
            builder.Register<InMemoryCampaignStateRepository>(Lifetime.Singleton).As<ICampaignStateRepository>();
            builder.Register<CampaignStepExecutorRegistry>(Lifetime.Singleton).As<ICampaignStepExecutorRegistry>().As<ICampaignStepExecutorRegistryWriter>();
            builder.Register<CampaignStepContextStore>(Lifetime.Singleton).As<ICampaignStepContextProvider>().As<ICampaignStepContextWriter>();
            builder.Register<CampaignRunner>(Lifetime.Singleton).As<ICampaignRunner>();
            builder.Register<CampaignManifestValidator>(Lifetime.Singleton);
            builder.Register<CampaignSession>(Lifetime.Singleton).As<ICampaignSession>();
            builder.Register<CampaignStateService>(Lifetime.Singleton).As<ICampaignStateReader>().As<ICampaignStateWriter>();
            builder.Register<SceneCampaignStepExecutor>(Lifetime.Singleton);
            builder.RegisterEntryPoint<CampaignStepExecutorRegistration>();

            builder.RegisterEntryPoint<CampaignBootstrapper>();
        }
    }
}
