﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.Processors;
using UnityEngine.Pool;
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

        /// <summary>
        /// Operation UI for Mobile.
        /// </summary>
        public ref MobileControlUIView UIView => ref uiView;

        protected virtual void OnEnable() => EnhancedTouchSupport.Enable();

        protected virtual void OnDisable() => EnhancedTouchSupport.Disable();

        protected override void Start()
        {
            base.Start();
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
            using (GenericPool<InvertVector2Processor>.Get(out var invertProcessor))
            {
                invertProcessor.invertX = invertX;
                invertProcessor.invertY = invertY;
                delta = invertProcessor.Process(delta, null);
            }
            using (GenericPool<ScaleVector2Processor>.Get(out var scaleProcessor))
            {
                scaleProcessor.x = scaleX;
                scaleProcessor.y = scaleY;
                delta = scaleProcessor.Process(delta, null);
            }
            base.OnActionTriggered(new CallbackContext(Look, InputActionPhase.Performed, delta));
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