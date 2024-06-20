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
        [Header("Invert Vector2")]
        [SerializeField]
        [Tooltip("If true, the <c>x</c> channel of the <c>Vector2</c> input value is inverted. True by default.")]
        private bool invertX = false;

        [SerializeField]
        [Tooltip("If true, the <c>y</c> channel of the <c>Vector2</c> input value is inverted. True by default.")]
        private bool invertY = true;

        [Header("Scale Vector2")]
        [SerializeField]
        [Tooltip("Scale factor to multiply the incoming Vector2's X component by.")]
        private float scaleX = 15.0f;

        [SerializeField]
        [Tooltip("Scale factor to multiply the incoming Vector2's Y component by.")]
        private float scaleY = 15.0f;

        [Header("Debug Options")]
        [SerializeField]
        [Tooltip("If true, the look action will be triggered when the mouse changes direction.")]
        private bool isDebug = false;

        protected virtual void OnEnable() => EnhancedTouchSupport.Enable();

        protected virtual void OnDisable() => EnhancedTouchSupport.Disable();

        protected override void Start()
        {
            base.Start();
            uiView.Joystick.OnValueChanged += value =>
            {
                const InputActionPhase phase = InputActionPhase.Performed;
                base.OnActionTriggered(new CallbackContext(MoveAction, phase, value));
            };
            uiView.SprintButton.OnStart += () =>
            {
                const InputActionPhase phase = InputActionPhase.Performed;
                base.OnActionTriggered(new CallbackContext(SprintAction, phase));
            };
            uiView.SprintButton.OnRelease += () =>
            {
                const InputActionPhase phase = InputActionPhase.Canceled;
                base.OnActionTriggered(new CallbackContext(SprintAction, phase));
            };
            uiView.JumpButton.onClick.AddListener(() =>
            {
                const InputActionPhase phase = InputActionPhase.Started;
                base.OnActionTriggered(new CallbackContext(JumpAction, phase));
            });
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
            Touch activeTouch = default;
            foreach (Touch touch in activeTouches)
            {
                if (avoidTouchIds.IndexOf(touch.touchId) == -1)
                {
                    activeTouch = touch;
                    break;
                }
            }
            if (activeTouch.Equals(default)) return;

            Vector2 delta = activeTouch.delta;
            ReadOnlySpan<InputProcessor<Vector2>> processors = new InputProcessor<Vector2>[]
            {
                new InvertVector2Processor { invertX = invertX, invertY = invertY },
                new ScaleVector2Processor { x = scaleX, y = scaleY }
            };

            foreach (InputProcessor<Vector2> processor in processors)
            {
                delta = processor.Process(delta, null);
            }
            base.OnActionTriggered(new CallbackContext(LookAction, InputActionPhase.Performed, delta));
        }

        protected override void OnActionTriggered(in CallbackContext context)
        {
            if (context.CompareActionName(LookAction))
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