namespace BerylliumMonoInput.Inputs.Keyboard;

public class KeyStatus(Keys key)
{
    public Keys Key { get; } = key;
    public ButtonStates State { get; internal set; } = ButtonStates.Pressed;
    internal bool IsHandled { get; set; } = true;
}
