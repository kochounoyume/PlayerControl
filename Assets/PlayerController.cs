using Unity.TinyCharacterController.Check;
using Unity.TinyCharacterController.Control;
using UnityEngine;
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
    public sealed class PlayerController : MonoBehaviour
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

        private readonly int speedAnim = Animator.StringToHash("Speed");

        private readonly int groundAnim = Animator.StringToHash("IsGround");

        private readonly int jumpStartAnim = Animator.StringToHash("JumpStart");

        private readonly int doubleJumpAnim = Animator.StringToHash("DoubleJump");

        private readonly int forwardAnim = Animator.StringToHash("Forward");

        private readonly int sideStepAnim = Animator.StringToHash("SideStep");

        private void Start()
        {
            playerInput.onActionTriggered += context =>
            {
                const string moveAction = "Move";
                const string sprintAction = "Sprint";
                const string jumpAction = "Jump";
                const string lookAction = "Look";

                switch (context.action.name)
                {
                    case moveAction when context.phase is InputActionPhase.Performed or InputActionPhase.Canceled:
                        moveControl.Move(context.ReadValue<Vector2>());
                        break;
                    case sprintAction when context.phase is InputActionPhase.Performed:
                        const float sprintHoldSpeed = 4.0f;
                        moveControl.MoveSpeed = sprintHoldSpeed;
                        break;
                    case sprintAction when context.phase is InputActionPhase.Canceled:
                        const float sprintReleasedSpeed = 1.2f;
                        moveControl.MoveSpeed = sprintReleasedSpeed;
                        break;
                    case jumpAction when context.phase is InputActionPhase.Started:
                        jumpControl.Jump();
                        break;
                    case lookAction when context.phase is InputActionPhase.Performed:
                        cameraControl.RotateCamera(context.ReadValue<Vector2>());
                        break;
                }
            };

            jumpControl.OnJump.AddListener(() => animator.Play(jumpControl.IsDoubleJump() ? doubleJumpAnim : jumpStartAnim));
        }

        private void Update()
        {
            animator.SetFloat(speedAnim, moveControl.CurrentSpeed);
            animator.SetBool(groundAnim, groundCheck.IsOnGround);

            Vector3 currentDirection = moveControl.LocalDirection;
            const float dampTime = 0.1f;
            float deltaTime = Time.deltaTime;
            animator.SetFloat(forwardAnim, currentDirection.z, dampTime, deltaTime);
            animator.SetFloat(sideStepAnim, currentDirection.x, dampTime, deltaTime);
        }
    }
}
