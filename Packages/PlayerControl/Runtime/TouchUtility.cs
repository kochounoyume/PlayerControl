using System.Runtime.CompilerServices;
using Vector2 = UnityEngine.Vector2;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace PlayerControl
{
    public static class TouchUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Touch GetApproximatelyActiveTouch(Vector2 screenPoint)
        {
            Touch result = default;
            float minSqrDistance = float.MaxValue;

            foreach (Touch touch in Touch.activeTouches)
            {
                Vector2 touchPos = touch.screenPosition;
                float distance = (touchPos - screenPoint).sqrMagnitude;

                if (distance < minSqrDistance)
                {
                    minSqrDistance = distance;
                    result = touch;
                }
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetApproximatelyActiveTouch(Vector2 screenPoint, out Touch touch)
        {
            touch = GetApproximatelyActiveTouch(screenPoint);
            return !touch.Equals(default);
        }
    }
}