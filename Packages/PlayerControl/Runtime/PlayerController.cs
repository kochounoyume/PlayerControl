using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.TinyCharacterController.Check;
using Unity.TinyCharacterController.Control;
using Unity.TinyCharacterController.Interfaces.Core;
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
    [RequireComponent(typeof(ITransform))]
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

        private new ITransform transform;

        private EventSystem eventSystem;

        private AnimHashConstant constant;

        private PointerEventData pointerEventData;

        protected ref readonly Animator Animator => ref animator;

        protected ref readonly PlayerInput PlayerInput => ref playerInput;

        protected ref readonly MoveControl MoveControl => ref moveControl;

        protected ref readonly JumpControl JumpControl => ref jumpControl;

        protected ref readonly GroundCheck GroundCheck => ref groundCheck;

        protected ref readonly TpsCameraControl CameraControl => ref cameraControl;

        protected ref readonly ITransform Transform => ref transform;

        protected ref readonly EventSystem EventSystem => ref eventSystem;

        /// <summary>
        /// "Speed" animation hash.
        /// </summary>
        protected ref readonly int SpeedAnim => ref constant.SpeedAnim;
        /// <summary>
        /// "IsGround" animation hash.
        /// </summary>
        protected ref readonly int GroundAnim => ref constant.GroundAnim;

        /// <summary>
        /// "JumpStart" animation hash.
        /// </summary>
        protected ref readonly int JumpStartAnim => ref constant.JumpStartAnim;

        /// <summary>
        /// "DoubleJump" animation hash.
        /// </summary>
        protected ref readonly int DoubleJumpAnim => ref constant.DoubleJumpAnim;

        /// <summary>
        /// "Forward" animation hash.
        /// </summary>
        protected ref readonly int ForwardAnim => ref constant.ForwardAnim;

        /// <summary>
        /// "SideStep" animation hash.
        /// </summary>
        protected ref readonly int SideStepAnim => ref constant.SideStepAnim;

        protected bool IsDoubleJump
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => JumpControl.AerialJumpCount >= 1;
        }

        protected float CurrentSpeed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => MoveControl.CurrentSpeed;
        }

        protected bool IsOnGround
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GroundCheck.IsOnGround;
        }

        protected Vector3 LocalDirection
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => MoveControl.LocalDirection;
        }

        protected Vector3 WorldPosition
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Transform.Position;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Transform.Position = value;
        }

        protected Quaternion WorldRotation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Transform.Rotation;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Transform.Rotation = value;
        }

        /// <summary>
        /// "Move" action name.
        /// </summary>
        public const string MoveAction = "Move";

        /// <summary>
        /// "Sprint" action name.
        /// </summary>
        public const string SprintAction = "Sprint";

        /// <summary>
        /// "Jump" action name.
        /// </summary>
        public const string JumpAction = "Jump";

        /// <summary>
        /// "Look" action name.
        /// </summary>
        public const string LookAction = "Look";

        /// <summary>
        /// The damper time for the move animation.
        /// </summary>
        public const float MoveDampTime = 0.1f;

        protected virtual void Start()
        {
            transform = GetComponent<ITransform>();
            eventSystem = EventSystem.current;
            constant = new AnimHashConstant(this);
            PlayerInput.onActionTriggered += context => OnActionTriggered(context);
            JumpControl.OnJump.AddListener(OnJump);
        }

        protected virtual void Update()
        {
            Animator.SetFloat(SpeedAnim, CurrentSpeed);
            Animator.SetBool(GroundAnim, IsOnGround);

            Vector3 currentDirection = LocalDirection;
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

        protected virtual void OnJump() => Animator.Play(IsDoubleJump ? DoubleJumpAnim : JumpStartAnim);

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
