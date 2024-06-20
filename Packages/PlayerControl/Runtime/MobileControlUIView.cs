using UnityEngine;
using UnityEngine.UI;

namespace PlayerControl
{
    /// <summary>
    /// Mobile control UI view class.
    /// </summary>
    public class MobileControlUIView : MonoBehaviour
    {
        [SerializeField]
        private MinimumVirtualJoyStick joystick = null;

        [SerializeField]
        private MinimumHoldButton sprintButton = null;

        [SerializeField]
        private Button jumpButton = null;

        /// <summary>
        /// Gets the joystick.
        /// </summary>
        public ref readonly MinimumVirtualJoyStick Joystick => ref joystick;

        /// <summary>
        /// Gets the sprint button.
        /// </summary>
        public ref readonly MinimumHoldButton SprintButton => ref sprintButton;

        /// <summary>
        /// Gets the jump button.
        /// </summary>
        public ref readonly Button JumpButton => ref jumpButton;
    }
}