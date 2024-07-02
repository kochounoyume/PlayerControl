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
        public readonly int SpeedAnim;

        /// <summary>
        /// "IsGround" animation hash.
        /// </summary>
        public readonly int GroundAnim;

        /// <summary>
        /// "JumpStart" animation hash.
        /// </summary>
        public readonly int JumpStartAnim;

        /// <summary>
        /// "DoubleJump" animation hash.
        /// </summary>
        public readonly int DoubleJumpAnim;

        /// <summary>
        /// "Forward" animation hash.
        /// </summary>
        public readonly int ForwardAnim;

        /// <summary>
        /// "SideStep" animation hash.
        /// </summary>
        public readonly int SideStepAnim;

        // Ensure that this class is not generated before Awake.
        public AnimHashConstants(in Object obj)
        {
            if (obj == null)
            {
                throw new System.ArgumentNullException(nameof(obj));
            }
            SpeedAnim = Animator.StringToHash("Speed");
            GroundAnim = Animator.StringToHash("IsGround");
            JumpStartAnim = Animator.StringToHash("JumpStart");
            DoubleJumpAnim = Animator.StringToHash("DoubleJump");
            ForwardAnim = Animator.StringToHash("Forward");
            SideStepAnim = Animator.StringToHash("SideStep");
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(nameof(AnimHashConstants));
            builder.Append(" {");
            builder.Append(nameof(SpeedAnim));
            builder.Append(" = ");
            builder.Append(SpeedAnim);
            builder.Append(", ");
            builder.Append(nameof(GroundAnim));
            builder.Append(" = ");
            builder.Append(GroundAnim);
            builder.Append(", ");
            builder.Append(nameof(JumpStartAnim));
            builder.Append(" = ");
            builder.Append(JumpStartAnim);
            builder.Append(", ");
            builder.Append(nameof(DoubleJumpAnim));
            builder.Append(" = ");
            builder.Append(DoubleJumpAnim);
            builder.Append(", ");
            builder.Append(nameof(ForwardAnim));
            builder.Append(" = ");
            builder.Append(ForwardAnim);
            builder.Append(", ");
            builder.Append(nameof(SideStepAnim));
            builder.Append(" = ");
            builder.Append(SideStepAnim);
            builder.Append(" }");
            return builder.ToString();
        }
    }
}
