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

        private Vector3 mStartPos;
        private Vector2 mPointerDownPos;

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

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background, eventData.position, eventData.pressEventCamera, out mPointerDownPos);
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

            Vector2 delta = Vector2.ClampMagnitude(position - mPointerDownPos, MovementRange);
            handle.anchoredPosition = (Vector2)mStartPos + delta;
            OnValueChanged?.Invoke(delta / MovementRange);
        }

        /// <inheritdoc />
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            handle.anchoredPosition = mPointerDownPos = mStartPos;
            OnValueChanged?.Invoke(Vector2.zero);
        }

        private void Start() => mStartPos = handle.anchoredPosition;
    }
}