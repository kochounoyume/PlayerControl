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
        protected Animator animator;

        [SerializeField]
        protected PlayerInput playerInput;

        [SerializeField]
        protected MoveControl moveControl;

        [SerializeField]
        protected JumpControl jumpControl;

        [SerializeField]
        protected GroundCheck groundCheck;

        [SerializeField]
        protected TpsCameraControl cameraControl;

        protected new ITransform transform;

        private AnimHashConstants constants;

        protected bool IsDoubleJump => jumpControl.AerialJumpCount >= 1;

        protected float CurrentSpeed => moveControl.CurrentSpeed;

        protected bool IsOnGround=> groundCheck.IsOnGround;

        protected Vector3 LocalDirection=> moveControl.LocalDirection;

        protected Vector3 WorldPosition
        {
            get => transform.Position;
            set => transform.Position = value;
        }

        protected Quaternion WorldRotation
        {
            get => transform.Rotation;
            set => transform.Rotation = value;
        }

        public AnimHashConstants Constants => constants ??= new AnimHashConstants();

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
            Debug.Log("110");
            transform = GetComponent<ITransform>();
            Debug.Log("114");
            playerInput.onActionTriggered += context => OnActionTriggered(context);
            Debug.Log("116");
            jumpControl.OnJump.AddListener(OnJump);
            Debug.Log("118");
        }

        protected virtual void Update()
        {
            Debug.Log("123");
            Debug.Log($"{nameof(CurrentSpeed)}は{moveControl == null}, {nameof(constants)}は{Constants == null}");
            animator.SetFloat(Constants!.Speed, CurrentSpeed);
            Debug.Log("125");
            animator.SetBool(Constants!.IsGround, IsOnGround);
            Debug.Log("127");

            Vector3 currentDirection = LocalDirection;
            Debug.Log("130");
            float deltaTime = Time.deltaTime;
            Debug.Log("132");
            animator.SetFloat(Constants!.Forward, currentDirection.z, MoveDampTime, deltaTime);
            Debug.Log("134");
            animator.SetFloat(Constants!.SideStep, currentDirection.x, MoveDampTime, deltaTime);
            Debug.Log("136");
        }

        protected virtual void OnActionTriggered(in CallbackContext context)
        {
            switch (context.ActionName)
            {
                case Move when context.Phase is InputActionPhase.Performed or InputActionPhase.Canceled:
                    moveControl.Move(context.Value);
                    break;
                case Sprint when context.Phase is InputActionPhase.Performed:
                    const float sprintHoldSpeed = 4.0f;
                    moveControl.MoveSpeed = sprintHoldSpeed;
                    break;
                case Sprint when context.Phase is InputActionPhase.Canceled:
                    const float sprintReleasedSpeed = 1.2f;
                    moveControl.MoveSpeed = sprintReleasedSpeed;
                    break;
                case Jump when context.Phase is InputActionPhase.Started:
                    jumpControl.Jump();
                    break;
                case Look when context.Phase is InputActionPhase.Performed:
                    cameraControl.RotateCamera(context.Value);
                    break;
            }
        }

        protected virtual void OnJump() => animator.Play(IsDoubleJump ? Constants.DoubleJump : Constants.JumpStart);

        /// <summary>
        /// Check if the pointer is hitting UI.
        /// </summary>
        /// <returns>True if the pointer is hitting UI.</returns>
        protected bool IsPointerHittingUI()
        {
            Pointer device = playerInput.GetDevice<Pointer>();
            if (device == null) return false;
            EventSystem eventSystem = EventSystem.current;
            PointerEventData pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = device.position.ReadValue();
            List<RaycastResult> results = new List<RaycastResult>();
            eventSystem.RaycastAll(pointerEventData, results);
            return results.Count > 0;
        }
    }
}
