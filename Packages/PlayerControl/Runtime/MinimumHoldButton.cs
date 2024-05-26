using System;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

namespace PlayerControl
{
    /// <summary>
    /// A button that sends an event when clicked and when released.
    /// </summary>
    public class MinimumHoldButton : Selectable
    {
        /// <summary>
        /// Whether the control is currently being used.
        /// </summary>
        public bool IsUsing { get; private set; } = false;

        /// <summary>
        /// The ID of the touch that is currently using the control.
        /// </summary>
        public int TouchId { get; private set; }

        /// <summary>
        /// Event triggered when the button is pressed.
        /// </summary>
        public event Action OnStart;

        /// <summary>
        /// Event triggered when the button is released.
        /// </summary>
        public event Action OnRelease;

        private EventSystem eventSystem;

        /// <inheritdoc />
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }

            base.OnPointerDown(eventData);

            IsUsing = true;
            if (TouchUtility.TryGetApproximatelyActiveTouch(eventData.position, out Touch touch))
            {
                TouchId = touch.touchId;
            }

            OnStart?.Invoke();

            Touch.onFingerUp += OnFingerUp;

            void OnFingerUp(Finger finger)
            {
                if (finger.currentTouch.touchId != TouchId) return;
                IsUsing = false;
                eventSystem ??= EventSystem.current;
                base.OnPointerUp(new PointerEventData(eventSystem) {button = PointerEventData.InputButton.Left});
                OnRelease?.Invoke();
                Touch.onFingerUp -= OnFingerUp;
            }
        }

        /// <inheritdoc />
        public override void OnPointerUp(PointerEventData eventData)
        {
            // Do nothing
        }

        protected override void OnEnable() => EnhancedTouchSupport.Enable();

        protected override void OnDisable() => EnhancedTouchSupport.Disable();
    }
}