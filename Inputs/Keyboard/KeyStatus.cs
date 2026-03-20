namespace BerylliumMonoInput.Inputs.Keyboard;

public class KeyStatus
{
    public Keys Key { get; }

    public ButtonStates KeyState
    {
        get;
        internal set
        {
            field = value;
            StateChanged = true;
        }
    }

    internal bool StateChanged { get; set; }

    public KeyStatus(Keys key)
    {
        Key = key;
        StateChanged = true;
    }
}
