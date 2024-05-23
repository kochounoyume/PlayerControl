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

        /// <summary>
        /// Compares the action name with the given string.
        /// </summary>
        /// <param name="other">target string</param>
        /// <returns>True if the action name is equal to the target string.</returns>
        public bool CompareActionName(ReadOnlySpan<char> other) => actionName.SequenceEqual(other);

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