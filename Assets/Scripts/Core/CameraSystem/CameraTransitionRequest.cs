namespace Geostorm.Core.CameraSystem
{
    public readonly struct CameraTransitionRequest
    {
        public static readonly CameraTransitionRequest Default = new(false, 0.0f);

        public bool ForceCut { get; }
        public float BlendDuration { get; }

        public CameraTransitionRequest(bool forceCut, float blendDuration)
        {
            ForceCut = forceCut;
            BlendDuration = blendDuration;
        }
    }
}