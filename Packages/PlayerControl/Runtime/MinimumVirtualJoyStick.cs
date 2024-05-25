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

        private Vector3 startPos;
        private Vector2 pointerDownPos;

        /// <summary>
        /// Whether the control is currently being used.
        /// </summary>
        public bool isUsing { get; private set; } = false;

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

            isUsing = true;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background, eventData.position, eventData.pressEventCamera, out pointerDownPos);
        }

        /// <inheritdoc />
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background, eventData.position, eventData.pressEventCamera, out Vector2 position);

            Vector2 delta = Vector2.ClampMagnitude(position - pointerDownPos, MovementRange);
            handle.anchoredPosition = (Vector2)startPos + delta;
            OnValueChanged?.Invoke(delta / MovementRange);
        }

        /// <inheritdoc />
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            handle.anchoredPosition = pointerDownPos = startPos;
            OnValueChanged?.Invoke(Vector2.zero);
            isUsing = false;
        }

        private void Start() => startPos = handle.anchoredPosition;
    }
}