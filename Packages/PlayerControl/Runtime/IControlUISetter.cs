namespace PlayerControl
{
    /// <summary>
    /// Control UI setter interface.
    /// </summary>
    public interface IControlUISetter
    {
        /// <summary>
        /// Set the UI view.
        /// </summary>
        MobileControlUIView UIView { set; }
    }
}