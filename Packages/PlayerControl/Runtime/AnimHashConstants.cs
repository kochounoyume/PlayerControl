using System.Text;
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
            StringBuilder builder = new StringBuilder(nameof(AnimHashConstants));
            builder.Append(" {");
            builder.Append(nameof(Speed));
            builder.Append(" = ");
            builder.Append(Speed);
            builder.Append(", ");
            builder.Append(nameof(IsGround));
            builder.Append(" = ");
            builder.Append(IsGround);
            builder.Append(", ");
            builder.Append(nameof(JumpStart));
            builder.Append(" = ");
            builder.Append(JumpStart);
            builder.Append(", ");
            builder.Append(nameof(DoubleJump));
            builder.Append(" = ");
            builder.Append(DoubleJump);
            builder.Append(", ");
            builder.Append(nameof(Forward));
            builder.Append(" = ");
            builder.Append(Forward);
            builder.Append(", ");
            builder.Append(nameof(SideStep));
            builder.Append(" = ");
            builder.Append(SideStep);
            builder.Append(" }");
            return builder.ToString();
        }
    }
}
