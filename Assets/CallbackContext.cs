using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public readonly ref struct CallbackContext
    {
        public readonly string ActionName;

        public readonly InputActionPhase Phase;

        public readonly Vector2 Value;

        public CallbackContext(string actionName, InputActionPhase phase, Vector2 value = default)
        {
            ActionName = actionName;
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