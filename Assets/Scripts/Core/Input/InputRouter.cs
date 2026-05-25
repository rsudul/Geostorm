using System.Collections.Generic;

namespace Geostorm.Core.Input
{
    public sealed class InputRouter
    {
        private readonly Stack<IInputContext> _contextStack = new();
        private readonly IInputProvider _inputProvider;

        public InputRouter(IInputProvider inputProvider)
        {
            _inputProvider = inputProvider;
        }

        public void PushContext(IInputContext context)
        {
            if (_contextStack.Count > 0)
            {
                _contextStack.Peek().OnDeactivated();
            }

            _contextStack.Push(context);
            context.OnActivated();
            _inputProvider.SwitchActionMap(context.ActionMapName);
        }

        public void PopContext()
        {
            if (_contextStack.Count > 0)
            {
                var popped = _contextStack.Pop();
                popped.OnDeactivated();
            }

            if (_contextStack.Count > 0)
            {
                var current = _contextStack.Peek();
                current.OnActivated();
                _inputProvider.SwitchActionMap(current.ActionMapName);
            }
            else
            {
                _inputProvider.SwitchActionMap("None");
            }
        }

        public IInputContext GetActiveContext()
        {
            return _contextStack.Count > 0 ? _contextStack.Peek() : null;
        }
    }
}