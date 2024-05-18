using System.Runtime.CompilerServices;
using Unity.TinyCharacterController.Control;

namespace PlayerControl
{
    public static class TCCExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDoubleJump(this JumpControl jumpControl)
        {
            return jumpControl.AerialJumpCount >= 1;
        }
    }
}