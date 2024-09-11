using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PlayerControl
{
    /// <summary>
    /// Animation hash constant.
    /// </summary>
    public sealed class AnimHashConstants
    {
        /// <summary>
        /// "Speed" animation hash.
        /// </summary>
        public readonly int Speed;

        /// <summary>
        /// "IsGround" animation hash.
        /// </summary>
        public readonly int IsGround;

        /// <summary>
        /// "JumpStart" animation hash.
        /// </summary>
        public readonly int JumpStart;

        /// <summary>
        /// "DoubleJump" animation hash.
        /// </summary>
        public readonly int DoubleJump;

        /// <summary>
        /// "Forward" animation hash.
        /// </summary>
        public readonly int Forward;

        /// <summary>
        /// "SideStep" animation hash.
        /// </summary>
        public readonly int SideStep;

        internal AnimHashConstants()
        {
            Speed = Animator.StringToHash(nameof(Speed));
            IsGround = Animator.StringToHash(nameof(IsGround));
            JumpStart = Animator.StringToHash(nameof(JumpStart));
            DoubleJump = Animator.StringToHash(nameof(DoubleJump));
            Forward = Animator.StringToHash(nameof(Forward));
            SideStep = Animator.StringToHash(nameof(SideStep));
        }

        public override string ToString()
        {
            int speed = GetDigits(Speed);
            int isGround = GetDigits(IsGround);
            int jumpStart = GetDigits(JumpStart);
            int doubleJump = GetDigits(DoubleJump);
            int forward = GetDigits(Forward);
            int sideStep = GetDigits(SideStep);
            int totalDigits = speed + isGround + jumpStart + doubleJump + forward + sideStep + 97;

            return string.Create(totalDigits, this, static (span, constants) =>
            {
                ReadOnlySpan<char> equals = stackalloc char[] { ' ', '=', ' ' };
                ReadOnlySpan<char> comma = stackalloc char[] { ',', ' ' };

                nameof(AnimHashConstants).AsSpan().CopyTo(span);
                int length = nameof(AnimHashConstants).Length;
                stackalloc char[] { ' ', '{', ' ' }.CopyTo(span.Slice(length));
                length += 3;
                nameof(Speed).AsSpan().CopyTo(span.Slice(length));
                length += nameof(Speed).Length;
                equals.CopyTo(span.Slice(length));
                length += equals.Length;
                constants.Speed.TryFormat(span.Slice(length), out int speedWritten);
                length += speedWritten;
                comma.CopyTo(span.Slice(length));
                length += comma.Length;
                nameof(IsGround).AsSpan().CopyTo(span.Slice(length));
                length += nameof(IsGround).Length;
                equals.CopyTo(span.Slice(length));
                length += equals.Length;
                constants.IsGround.TryFormat(span.Slice(length), out int isGroundWritten);
                length += isGroundWritten;
                comma.CopyTo(span.Slice(length));
                length += comma.Length;
                nameof(JumpStart).AsSpan().CopyTo(span.Slice(length));
                length += nameof(JumpStart).Length;
                equals.CopyTo(span.Slice(length));
                length += equals.Length;
                constants.JumpStart.TryFormat(span.Slice(length), out int jumpStartWritten);
                length += jumpStartWritten;
                comma.CopyTo(span.Slice(length));
                length += comma.Length;
                nameof(DoubleJump).AsSpan().CopyTo(span.Slice(length));
                length += nameof(DoubleJump).Length;
                equals.CopyTo(span.Slice(length));
                length += equals.Length;
                constants.DoubleJump.TryFormat(span.Slice(length), out int doubleJumpWritten);
                length += doubleJumpWritten;
                comma.CopyTo(span.Slice(length));
                length += comma.Length;
                nameof(Forward).AsSpan().CopyTo(span.Slice(length));
                length += nameof(Forward).Length;
                equals.CopyTo(span.Slice(length));
                length += equals.Length;
                constants.Forward.TryFormat(span.Slice(length), out int forwardWritten);
                length += forwardWritten;
                comma.CopyTo(span.Slice(length));
                length += comma.Length;
                nameof(SideStep).AsSpan().CopyTo(span.Slice(length));
                length += nameof(SideStep).Length;
                equals.CopyTo(span.Slice(length));
                length += equals.Length;
                constants.SideStep.TryFormat(span.Slice(length), out int sideStepWritten);
                length += sideStepWritten;
                stackalloc char[] { ' ', '}' }.CopyTo(span.Slice(length));
            });

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static int GetDigits(in int value) => (int)MathF.Log10(Math.Abs(value)) + 1 + (value < 0 ? 1 : 0);
        }
    }
}
