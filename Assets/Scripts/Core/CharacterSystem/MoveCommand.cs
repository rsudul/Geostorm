using UnityEngine;
using UnityEngine.Pool;

namespace Geostorm.Core.CharacterSystem
{
    public sealed class MoveCommand : ICommand
    {
        private static readonly ObjectPool<MoveCommand> _pool = new ObjectPool<MoveCommand>(
            createFunc: () => new MoveCommand(),
            actionOnGet: cmd => { },
            actionOnRelease: cmd => cmd.Reset(),
            actionOnDestroy: null,
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 100
        );

        public float ExpirationTime { get; private set; }
        public Vector3 Direction { get; private set; }

        private MoveCommand() { }

        public static MoveCommand Get(Vector3 direction, float expirationTime = 0.0f)
        {
            var cmd = _pool.Get();
            cmd.Direction = direction;
            cmd.ExpirationTime = expirationTime;
            return cmd;
        }

        public bool Execute(IPawn pawn)
        {
            pawn.Move(Direction);
            return true;
        }

        public void Release()
        {
            _pool.Release(this);
        }

        private void Reset()
        {
            Direction = Vector3.zero;
            ExpirationTime = 0.0f;
        }
    }
}