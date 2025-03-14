using System.Runtime.CompilerServices;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace PlayerControl
{
    public static class TouchUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Touch GetApproximatelyActiveTouch(in Vector2 screenPoint)
        {
            (Touch result, float minSqrDistance) = (default, float.MaxValue);

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
        public static bool TryGetApproximatelyActiveTouch(in Vector2 screenPoint, out Touch touch)
        {
            touch = GetApproximatelyActiveTouch(screenPoint);
            return !touch.Equals(default);
        }
    }
}