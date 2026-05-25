using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Geostorm.Core.CameraSystem;
using Geostorm.Core.CharacterSystem;
using Geostorm.Core.Input;
using Geostorm.Core.SceneManagement;
using Geostorm.Infrastructure.CameraSystem;
using Geostorm.Infrastructure.Input;
using Geostorm.Infrastructure.SceneManagement;

namespace Geostorm.Infrastructure
{
    public sealed class RootLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private SceneConfiguration _sceneConfiguration;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_sceneConfiguration);

            builder.Register<UnityInputProvider>(Lifetime.Singleton).As<IInputProvider, IDisposable>();
            builder.Register<InputRouter>(Lifetime.Singleton);

            builder.Register<PossessionManager>(Lifetime.Singleton);

            builder.Register<CameraStack>(Lifetime.Singleton);
            builder.Register<CameraRigRegistry>(Lifetime.Singleton);
            builder.Register<CameraDirector>(Lifetime.Singleton).As<ICameraDirector>().As<IViewFrameProvider>().AsSelf();
            builder.RegisterEntryPoint<CameraSystemUpdater>();

            builder.Register<UnitySceneLoader>(Lifetime.Singleton).As<ISceneLoader>();

            builder.RegisterEntryPoint<Bootstrapper>();
        }
    }
}