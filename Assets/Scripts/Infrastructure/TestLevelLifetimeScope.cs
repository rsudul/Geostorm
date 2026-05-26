using System;
using VContainer;
using VContainer.Unity;
using Geostorm.Infrastructure.CameraSystem;
using Geostorm.Infrastructure.CharacterSystem;

namespace Geostorm.Infrastructure
{
    public sealed class TestLevelLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PossessionCameraBinder>();
            builder.RegisterEntryPoint<CameraModeSwitchingController>();

            builder.Register<PlayerBrain>(Lifetime.Transient);
            builder.Register<AIBrain>(Lifetime.Transient);
            builder.Register<Func<AIBrain>>(container => () => container.Resolve<AIBrain>(), Lifetime.Transient);

            builder.RegisterEntryPoint<TestLevelInitializer>();
        }
    }
}