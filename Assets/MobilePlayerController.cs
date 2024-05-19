using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl.Mobile
{
    public class MobilePlayerController : PlayerController
    {
        [SerializeField]
        private MinimumVirtualJoyStick joystick = null;

        protected override void Start()
        {
            base.Start();
            joystick.OnValueChanged += value =>
            {
                const InputActionPhase phase = InputActionPhase.Performed;
                OnActionTriggered(new CallbackContext(MoveAction, phase, value));
            };
        }
    }
}