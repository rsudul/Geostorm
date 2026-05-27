using UnityEngine.Pool;

namespace Geostorm.Core.CharacterSystem
{
    public sealed class YawInputCommand : ICommand
    {
        private static readonly ObjectPool<YawInputCommand> _pool = new(
            createFunc: () => new YawInputCommand(),
            actionOnGet: null,
            actionOnRelease: cmd => cmd.Reset(),
            actionOnDestroy: null,
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 100
        );

        public float ExpirationTime { get; private set; }
        public float YawInput { get; private set; }

        private YawInputCommand()
        {

        }

        public static YawInputCommand Get(float yawInput)
        {
            YawInputCommand cmd = _pool.Get();
            cmd.YawInput = yawInput;
            cmd.ExpirationTime = 0.0f;
            return cmd;
        }

        public bool Execute(IPawn pawn)
        {
            pawn.AddYawInput(YawInput);
            return true;
        }

        public void Release()
        {
            _pool.Release(this);
        }

        private void Reset()
        {
            YawInput = 0.0f;
            ExpirationTime = 0.0f;
        }
    }
}