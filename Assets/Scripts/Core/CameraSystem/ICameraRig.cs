namespace Geostorm.Core.CameraSystem
{
    public interface ICameraRig
    {
        CameraModeId ModeId { get; }
        bool IsActive { get; }
        void BindTargetProvider(ICameraTargetProvider targetProvider);
        void Activate(CameraTransitionRequest transitionRequest);
        void Deactivate(CameraTransitionRequest transitionRequest);
        void Tick(float deltaTime);
    }
}