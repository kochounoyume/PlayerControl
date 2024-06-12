using System.Collections.Generic;
using Unity.TinyCharacterController.Check;
using Unity.TinyCharacterController.Control;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    /// <summary>
    /// Player controller.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(MoveControl))]
    [RequireComponent(typeof(JumpControl))]
    [RequireComponent(typeof(GroundCheck))]
    [RequireComponent(typeof(TpsCameraControl))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private PlayerInput playerInput;

        [SerializeField]
        private MoveControl moveControl;

        [SerializeField]
        private JumpControl jumpControl;

        [SerializeField]
        private GroundCheck groundCheck;

        [SerializeField]
        private TpsCameraControl cameraControl;

        private EventSystem eventSystem;

        private PointerEventData pointerEventData;

        protected const string MoveAction = "Move";

        protected const string SprintAction = "Sprint";

        protected const string JumpAction = "Jump";

        protected const string LookAction = "Look";

        protected const float MoveDampTime = 0.1f;

        protected readonly int SpeedAnim = Animator.StringToHash("Speed");

        protected readonly int GroundAnim = Animator.StringToHash("IsGround");

        protected readonly int JumpStartAnim = Animator.StringToHash("JumpStart");

        protected readonly int DoubleJumpAnim = Animator.StringToHash("DoubleJump");

        protected readonly int ForwardAnim = Animator.StringToHash("Forward");

        protected readonly int SideStepAnim = Animator.StringToHash("SideStep");

        protected ref readonly Animator Animator => ref animator;

        protected ref readonly PlayerInput PlayerInput => ref playerInput;

        protected ref readonly MoveControl MoveControl => ref moveControl;

        protected ref readonly JumpControl JumpControl => ref jumpControl;

        protected ref readonly GroundCheck GroundCheck => ref groundCheck;

        protected ref readonly TpsCameraControl CameraControl => ref cameraControl;

        protected ref readonly EventSystem EventSystem => ref eventSystem;

        protected virtual void Start()
        {
            eventSystem = EventSystem.current;
            PlayerInput.onActionTriggered += context => OnActionTriggered(context);
            JumpControl.OnJump.AddListener(OnJump);
        }

        protected virtual void Update()
        {
            Animator.SetFloat(SpeedAnim, MoveControl.CurrentSpeed);
            Animator.SetBool(GroundAnim, GroundCheck.IsOnGround);

            Vector3 currentDirection = MoveControl.LocalDirection;
            float deltaTime = Time.deltaTime;
            Animator.SetFloat(ForwardAnim, currentDirection.z, MoveDampTime, deltaTime);
            Animator.SetFloat(SideStepAnim, currentDirection.x, MoveDampTime, deltaTime);
        }

        protected virtual void OnActionTriggered(in CallbackContext context)
        {
            switch (context.ActionName)
            {
                case MoveAction when context.Phase is InputActionPhase.Performed or InputActionPhase.Canceled:
                    MoveControl.Move(context.Value);
                    break;
                case SprintAction when context.Phase is InputActionPhase.Performed:
                    const float sprintHoldSpeed = 4.0f;
                    MoveControl.MoveSpeed = sprintHoldSpeed;
                    break;
                case SprintAction when context.Phase is InputActionPhase.Canceled:
                    const float sprintReleasedSpeed = 1.2f;
                    MoveControl.MoveSpeed = sprintReleasedSpeed;
                    break;
                case JumpAction when context.Phase is InputActionPhase.Started:
                    JumpControl.Jump();
                    break;
                case LookAction when context.Phase is InputActionPhase.Performed:
                    CameraControl.RotateCamera(context.Value);
                    break;
            }
        }

        protected virtual void OnJump() => Animator.Play(JumpControl.IsDoubleJump() ? DoubleJumpAnim : JumpStartAnim);

        /// <summary>
        /// Check if the pointer is hitting UI.
        /// </summary>
        /// <returns>True if the pointer is hitting UI.</returns>
        protected bool IsPointerHittingUI()
        {
            Pointer device = playerInput.GetDevice<Pointer>();
            if (device == null) return false;
            pointerEventData ??= new PointerEventData(eventSystem);
            pointerEventData.position = device.position.ReadValue();
            List<RaycastResult> results = new List<RaycastResult>();
            eventSystem.RaycastAll(pointerEventData, results);
            return results.Count > 0;
        }
    }
}
