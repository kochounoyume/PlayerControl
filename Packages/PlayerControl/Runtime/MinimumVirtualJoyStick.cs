using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerControl
{
    [RequireComponent(typeof(RectTransform))]
    public class MinimumVirtualJoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField, Min(0)]
        private float movementRange = 50;

        [SerializeField]
        private RectTransform handle;

        [SerializeField]
        private RectTransform background;

        private Vector2 startScreenPos;
        private Vector2 handleScreenPos;

        private Vector2 startPos;
        private Vector2 pointerDownPos;

        /// <summary>
        /// Callback executed when the value of the control changes.
        /// </summary>
        public event Action<Vector2> OnValueChanged;

        /// <summary>
        /// The distance from the onscreen control's center of origin, around which the control can move.
        /// </summary>
        public ref float MovementRange => ref movementRange;

        /// <summary>
        /// The current screen position of the control's handle.
        /// </summary>
        public ref readonly Vector2 HandleScreenPos => ref handleScreenPos;

        /// <inheritdoc />
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }

            handleScreenPos = eventData.position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background, handleScreenPos, eventData.pressEventCamera, out pointerDownPos);
        }

        /// <inheritdoc />
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }

            handleScreenPos = eventData.position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background, handleScreenPos, eventData.pressEventCamera, out Vector2 localPos);

            Vector2 delta = Vector2.ClampMagnitude(localPos - pointerDownPos, MovementRange);
            handle.anchoredPosition = startPos + delta;
            OnValueChanged?.Invoke(delta / MovementRange);
        }

        /// <inheritdoc />
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            handle.anchoredPosition = pointerDownPos = startPos;
            handleScreenPos = startScreenPos;
            OnValueChanged?.Invoke(Vector2.zero);
        }

        private void Awake()
        {
            startPos = handle.anchoredPosition;
            Canvas canvas = GetComponentInParent<Canvas>();
            Camera pressEventCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            startScreenPos = handleScreenPos = RectTransformUtility.WorldToScreenPoint(pressEventCamera, handle.position);
        }
    }
}