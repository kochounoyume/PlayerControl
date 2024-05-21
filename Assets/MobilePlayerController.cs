using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PlayerControl.Mobile
{
    public class MobilePlayerController : PlayerController
    {
        [SerializeField]
        private MinimumVirtualJoyStick joystick = null;

        [SerializeField]
        private MinimumHoldButton sprintButton = null;

        [SerializeField]
        private Button jumpButton = null;

        protected override void Start()
        {
            base.Start();
            joystick.OnValueChanged += value =>
            {
                const InputActionPhase phase = InputActionPhase.Performed;
                base.OnActionTriggered(new CallbackContext(MoveAction, phase, value));
            };
            sprintButton.OnStart += () =>
            {
                const InputActionPhase phase = InputActionPhase.Performed;
                base.OnActionTriggered(new CallbackContext(SprintAction, phase));
            };
            sprintButton.OnRelease += () =>
            {
                const InputActionPhase phase = InputActionPhase.Canceled;
                base.OnActionTriggered(new CallbackContext(SprintAction, phase));
            };
            jumpButton.onClick.AddListener(() =>
            {
                const InputActionPhase phase = InputActionPhase.Started;
                base.OnActionTriggered(new CallbackContext(JumpAction, phase));
            });
        }

        protected override void OnActionTriggered(CallbackContext context)
        {
            if (context.ActionName == LookAction && joystick.isUsing)
            {
                return;
            }
            base.OnActionTriggered(context);
        }
    }
}