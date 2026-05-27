using UnityEngine.Pool;

namespace Geostorm.Core.CharacterSystem
{
    public sealed class SetRotationModeCommand : ICommand
    {
        private static readonly ObjectPool<SetRotationModeCommand> _pool = new(
            createFunc: () => new SetRotationModeCommand(),
            actionOnGet: null,
            actionOnRelease: cmd => cmd.Reset(),
            actionOnDestroy: null,
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 100
        );

        public float ExpirationTime { get; private set; }
        public PawnRotationMode RotationMode { get; private set; }

        private SetRotationModeCommand()
        {

        }

        public static SetRotationModeCommand Get(PawnRotationMode rotationMode)
        {
            SetRotationModeCommand cmd = _pool.Get();
            cmd.RotationMode = rotationMode;
            cmd.ExpirationTime = 0.0f;
            return cmd;
        }

        public bool Execute(IPawn pawn)
        {
            pawn.SetRotationMode(RotationMode);
            return true;
        }

        public void Release()
        {
            _pool.Release(this);
        }

        private void Reset()
        {
            RotationMode = PawnRotationMode.MovementDirection;
            ExpirationTime = 0.0f;
        }
    }
}