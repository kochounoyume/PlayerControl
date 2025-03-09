using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.Processors;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace PlayerControl
{
    public class MobilePlayerController : PlayerController
    {
        [SerializeField]
        private MobileControlUIView uiView;

        [Header("Processors")]
        [SerializeReference]
        private InvertVector2Processor invertProcessor = new () { invertX = false, invertY = true };

        [SerializeReference]
        private ScaleVector2Processor scaleProcessor = new () { x = 15.0f, y = 15.0f };

        [Header("Debug Options")]
        [SerializeField]
        [Tooltip("If true, the look action will be triggered when the mouse changes direction.")]
        private bool isDebug = false;

        /// <summary>
        /// Operation UI for Mobile.
        /// </summary>
        public virtual MobileControlUIView UIView
        {
            get => uiView;
            set
            {
                uiView = value;
                uiView.Joystick.OnValueChanged += value =>
                {
                    base.OnActionTriggered(new CallbackContext(Move, InputActionPhase.Performed, value));
                };
                uiView.SprintButton.OnStart += () =>
                {
                    base.OnActionTriggered(new CallbackContext(Sprint, InputActionPhase.Performed));
                };
                uiView.SprintButton.OnRelease += () =>
                {
                    base.OnActionTriggered(new CallbackContext(Sprint, InputActionPhase.Canceled));
                };
                uiView.JumpButton.onClick.AddListener(() =>
                {
                    base.OnActionTriggered(new CallbackContext(Jump, InputActionPhase.Started));
                });
            }
        }

        protected virtual void OnEnable() => EnhancedTouchSupport.Enable();

        protected virtual void OnDisable() => EnhancedTouchSupport.Disable();

        protected override void Start()
        {
            base.Start();
            if (uiView != null)
            {
                UIView = uiView;
            }
        }

        protected override void Update()
        {
            base.Update();
            var activeTouches = Touch.activeTouches;
            ReadOnlySpan<int> avoidTouchIds = (uiView.Joystick.IsUsing, uiView.SprintButton.IsUsing) switch
            {
                (true, true) => stackalloc int[] { uiView.Joystick.TouchId, uiView.SprintButton.TouchId },
                (true, false) => stackalloc int[] { uiView.Joystick.TouchId },
                (false, true) => stackalloc int[] { uiView.SprintButton.TouchId },
                _ => ReadOnlySpan<int>.Empty
            };
            if (activeTouches.Count <= avoidTouchIds.Length) return;
            foreach (Touch touch in activeTouches)
            {
                if (avoidTouchIds.IndexOf(touch.touchId) == -1)
                {
                    Vector2 delta = touch.delta;
                    delta = invertProcessor.Process(delta, null);
                    delta = scaleProcessor.Process(delta, null);
                    base.OnActionTriggered(new CallbackContext(Look, InputActionPhase.Performed, delta));
                    break;
                }
            }
        }

        protected override void OnActionTriggered(in CallbackContext context)
        {
            if (context.CompareActionName(Look))
            {
#if UNITY_EDITOR
                if (!isDebug)
                {
                    return;
                }
#else
                return;
#endif
            }
            base.OnActionTriggered(context);
        }
    }
}