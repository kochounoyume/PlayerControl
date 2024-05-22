using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlayerControl
{
    /// <summary>
    /// A button that sends an event when clicked and when released.
    /// </summary>
    public class MinimumHoldButton : Selectable
    {
        /// <summary>
        /// Event triggered when the button is pressed.
        /// </summary>
        public event Action OnStart;

        /// <summary>
        /// Event triggered when the button is released.
        /// </summary>
        public event Action OnRelease;

        /// <inheritdoc />
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            OnStart?.Invoke();
        }

        /// <inheritdoc />
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            OnRelease?.Invoke();
        }
    }
}