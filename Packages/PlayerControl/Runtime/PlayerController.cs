using System.Collections.Generic;
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

        private AnimHashConstants constants;

        private PointerEventData pointerEventData;

        protected ref readonly Animator Animator => ref animator;

        protected ref readonly PlayerInput PlayerInput => ref playerInput;

        protected ref readonly MoveControl MoveControl => ref moveControl;

        protected ref readonly JumpControl JumpControl => ref jumpControl;

        protected ref readonly GroundCheck GroundCheck => ref groundCheck;

        protected ref readonly TpsCameraControl CameraControl => ref cameraControl;

        protected ref readonly ITransform Transform => ref transform;

        protected ref readonly AnimHashConstants Constants => ref constants;

        protected bool IsDoubleJump => JumpControl.AerialJumpCount >= 1;

        protected float CurrentSpeed => MoveControl.CurrentSpeed;

        protected bool IsOnGround=> GroundCheck.IsOnGround;

        protected Vector3 LocalDirection=> MoveControl.LocalDirection;

        protected Vector3 WorldPosition
        {
            get => Transform.Position;
            set => Transform.Position = value;
        }

        protected Quaternion WorldRotation
        {
            get => Transform.Rotation;
            set => Transform.Rotation = value;
        }

        /// <summary>
        /// "Move" action name.
        /// </summary>
        public const string Move = nameof(Move);

        /// <summary>
        /// "Sprint" action name.
        /// </summary>
        public const string Sprint = nameof(Sprint);

        /// <summary>
        /// "Jump" action name.
        /// </summary>
        public const string Jump = nameof(Jump);

        /// <summary>
        /// "Look" action name.
        /// </summary>
        public const string Look = nameof(Look);

        /// <summary>
        /// The damper time for the move animation.
        /// </summary>
        public const float MoveDampTime = 0.1f;

        protected virtual void Start()
        {
            transform = GetComponent<ITransform>();
            constants = new AnimHashConstants();
            PlayerInput.onActionTriggered += context => OnActionTriggered(context);
            JumpControl.OnJump.AddListener(OnJump);
        }

        protected virtual void Update()
        {
            Animator.SetFloat(constants.Speed, CurrentSpeed);
            Animator.SetBool(constants.IsGround, IsOnGround);

            Vector3 currentDirection = LocalDirection;
            float deltaTime = Time.deltaTime;
            Animator.SetFloat(constants.Forward, currentDirection.z, MoveDampTime, deltaTime);
            Animator.SetFloat(constants.SideStep, currentDirection.x, MoveDampTime, deltaTime);
        }

        protected virtual void OnActionTriggered(in CallbackContext context)
        {
            switch (context.ActionName)
            {
                case Move when context.Phase is InputActionPhase.Performed or InputActionPhase.Canceled:
                    MoveControl.Move(context.Value);
                    break;
                case Sprint when context.Phase is InputActionPhase.Performed:
                    const float sprintHoldSpeed = 4.0f;
                    MoveControl.MoveSpeed = sprintHoldSpeed;
                    break;
                case Sprint when context.Phase is InputActionPhase.Canceled:
                    const float sprintReleasedSpeed = 1.2f;
                    MoveControl.MoveSpeed = sprintReleasedSpeed;
                    break;
                case Jump when context.Phase is InputActionPhase.Started:
                    JumpControl.Jump();
                    break;
                case Look when context.Phase is InputActionPhase.Performed:
                    CameraControl.RotateCamera(context.Value);
                    break;
            }
        }

        protected virtual void OnJump() => Animator.Play(IsDoubleJump ? constants.DoubleJump : constants.JumpStart);

        /// <summary>
        /// Check if the pointer is hitting UI.
        /// </summary>
        /// <returns>True if the pointer is hitting UI.</returns>
        protected bool IsPointerHittingUI()
        {
            Pointer device = playerInput.GetDevice<Pointer>();
            if (device == null) return false;
            EventSystem eventSystem = EventSystem.current;
            pointerEventData ??= new PointerEventData(eventSystem);
            pointerEventData.position = device.position.ReadValue();
            List<RaycastResult> results = new List<RaycastResult>();
            eventSystem.RaycastAll(pointerEventData, results);
            return results.Count > 0;
        }
    }
}
