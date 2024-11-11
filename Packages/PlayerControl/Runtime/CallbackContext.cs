using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public readonly ref struct CallbackContext
    {
        public readonly InputActionPhase Phase;
        public readonly Vector2 Value;
        public readonly string ActionName;

        /// <summary>
        /// Compares the action name with the given string.
        /// </summary>
        /// <param name="other">target string</param>
        /// <returns>True if the action name is equal to the target string.</returns>
        public bool CompareActionName(in string other) => ActionName == other;

        public CallbackContext(in string actionName, in InputActionPhase phase, in Vector2 value = default)
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