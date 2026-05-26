namespace Geostorm.Core.CameraSystem
{
    public interface ICameraDirector
    {
        CameraModeId CurrentModeId { get; }
        void SetTargetProvider(ICameraTargetProvider targetProvider);
        void SetBaseMode(CameraModeId modeId, CameraTransitionRequest transitionRequest);
        void PushOverride(CameraModeId modeId, CameraTransitionRequest transitionRequest);
        void PopOverride(CameraModeId modeId, CameraTransitionRequest transitionRequest);
        void ClearOverrides(CameraTransitionRequest transitionRequest);
        void Tick(float deltaTime);
        CameraDebugState GetDebugState();
    }
}