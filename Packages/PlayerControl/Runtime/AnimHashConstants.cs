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
        public readonly int SpeedAnim = Animator.StringToHash("Speed");

        /// <summary>
        /// "IsGround" animation hash.
        /// </summary>
        public readonly int GroundAnim = Animator.StringToHash("IsGround");

        /// <summary>
        /// "JumpStart" animation hash.
        /// </summary>
        public readonly int JumpStartAnim = Animator.StringToHash("JumpStart");

        /// <summary>
        /// "DoubleJump" animation hash.
        /// </summary>
        public readonly int DoubleJumpAnim = Animator.StringToHash("DoubleJump");

        /// <summary>
        /// "Forward" animation hash.
        /// </summary>
        public readonly int ForwardAnim = Animator.StringToHash("Forward");

        /// <summary>
        /// "SideStep" animation hash.
        /// </summary>
        public readonly int SideStepAnim = Animator.StringToHash("SideStep");

        // Ensure that this class is not generated before Awake.
        public AnimHashConstants(in Object obj)
        {
        }
    }
}