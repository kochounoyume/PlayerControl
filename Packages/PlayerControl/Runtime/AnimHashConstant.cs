using UnityEngine;

namespace PlayerControl
{
    /// <summary>
    /// Animation hash constant.
    /// </summary>
    internal sealed class AnimHashConstant
    {
        /// <summary>
        /// "Speed" animation hash.
        /// </summary>
        internal readonly int SpeedAnim = Animator.StringToHash("Speed");

        /// <summary>
        /// "IsGround" animation hash.
        /// </summary>
        internal readonly int GroundAnim = Animator.StringToHash("IsGround");

        /// <summary>
        /// "JumpStart" animation hash.
        /// </summary>
        internal readonly int JumpStartAnim = Animator.StringToHash("JumpStart");

        /// <summary>
        /// "DoubleJump" animation hash.
        /// </summary>
        internal readonly int DoubleJumpAnim = Animator.StringToHash("DoubleJump");

        /// <summary>
        /// "Forward" animation hash.
        /// </summary>
        internal readonly int ForwardAnim = Animator.StringToHash("Forward");

        /// <summary>
        /// "SideStep" animation hash.
        /// </summary>
        internal readonly int SideStepAnim = Animator.StringToHash("SideStep");

        // Ensure that this class is not generated before Awake.
        internal AnimHashConstant(in Object obj)
        {
        }
    }
}