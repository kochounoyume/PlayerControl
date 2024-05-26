using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace PlayerControl
{
    [RequireComponent(typeof(RectTransform))]
    public class MinimumVirtualJoyStick : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField, Min(0)]
        private float movementRange = 50;

        [SerializeField]
        private RectTransform handle;

        [SerializeField]
        private RectTransform background;

        private Vector3 startPos;
        private Vector2 pointerDownPos;
        private Camera pressEventCamera;

        /// <summary>
        /// The ID of the touch that is currently using the control.
        /// </summary>
        public int TouchId { get; private set; }

        /// <summary>
        /// Whether the control is currently being used.
        /// </summary>
        public bool IsUsing { get; private set; } = false;

        /// <summary>
        /// Callback executed when the value of the control changes.
        /// </summary>
        public event Action<Vector2> OnValueChanged;

        /// <summary>
        /// The distance from the onscreen control's center of origin, around which the control can move.
        /// </summary>
        public ref float MovementRange => ref movementRange;

        /// <inheritdoc />
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }

            IsUsing = true;
            pressEventCamera = eventData.pressEventCamera;

            Vector2 screenPoint = eventData.position;
            if (TouchUtility.TryGetApproximatelyActiveTouch(screenPoint, out Touch touch))
            {
                screenPoint = touch.screenPosition;
                TouchId = touch.touchId;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background, screenPoint, pressEventCamera, out pointerDownPos);

            Touch.onFingerMove += OnFingerMove;

            Touch.onFingerUp += OnFingerUp;

            void OnFingerMove(Finger finger)
            {
                if (finger.currentTouch.touchId == TouchId)
                {
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        background, finger.screenPosition, pressEventCamera, out Vector2 position);

                    Vector2 delta = Vector2.ClampMagnitude(position - pointerDownPos, MovementRange);
                    handle.anchoredPosition = (Vector2)startPos + delta;
                    OnValueChanged?.Invoke(delta / MovementRange);
                }
            }

            void OnFingerUp(Finger finger)
            {
                if (finger.currentTouch.touchId == TouchId)
                {
                    handle.anchoredPosition = pointerDownPos = startPos;
                    OnValueChanged?.Invoke(Vector2.zero);
                    IsUsing = false;
                    Touch.onFingerMove -= OnFingerMove;
                    Touch.onFingerUp -= OnFingerUp;
                }
            }
        }

        private void Start() => startPos = handle.anchoredPosition;

        private void OnEnable() => EnhancedTouchSupport.Enable();

        private void OnDisable() => EnhancedTouchSupport.Disable();
    }
}