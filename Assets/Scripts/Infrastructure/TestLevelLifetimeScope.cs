using System;
using VContainer;
using VContainer.Unity;
using Geostorm.Core.CameraSystem;
using Geostorm.Core.CharacterSystem;
using Geostorm.Core.Input;
using Geostorm.Infrastructure.CameraSystem;
using Geostorm.Infrastructure.CharacterSystem;
using Geostorm.Infrastructure.Input;

namespace Geostorm.Infrastructure
{
    public sealed class TestLevelLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PossessionCameraBinder>();
            builder.Register<CameraModeSwitchingController>(Lifetime.Singleton).As<ICameraModeSwitcher>();

            builder.Register<PlayerBrain>(Lifetime.Singleton).AsSelf().As<IPlayerInputIntentReceiver>();
            builder.Register<AIBrain>(Lifetime.Transient);
            builder.Register<Func<AIBrain>>(container => () => container.Resolve<AIBrain>(), Lifetime.Transient);
            builder.Register<PlayerPossessionController>(Lifetime.Singleton).AsSelf().As<IPlayerCharacterSwitcher>();

            builder.RegisterEntryPoint<TestLevelInitializer>();
            builder.RegisterEntryPoint<GameplayInputController>();
        }
    }
}