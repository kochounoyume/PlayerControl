using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public readonly ref struct CallbackContext
    {
        private readonly ReadOnlySpan<char> actionName;

        public readonly InputActionPhase Phase;

        public readonly Vector2 Value;

        public string ActionName => actionName.ToString();

        public CallbackContext(ReadOnlySpan<char> actionName, InputActionPhase phase, Vector2 value = default)
        {
            this.actionName = actionName;
            Phase = phase;
            Value = value;
        }

        public static implicit operator CallbackContext(InputAction.CallbackContext context)
        {
            return context.valueType == typeof(Vector2)
                ? new CallbackContext(context.action.name, context.phase, context.ReadValue<Vector2>())
                : new CallbackContext(context.action.name, context.phase);
        }
    }
}