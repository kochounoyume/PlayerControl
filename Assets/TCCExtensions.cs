using Unity.TinyCharacterController.Control;

namespace PlayerControl
{
    public static class TCCExtensions
    {
        public static bool IsDoubleJump(this JumpControl jumpControl)
        {
            return jumpControl.AerialJumpCount >= 1;
        }
    }
}