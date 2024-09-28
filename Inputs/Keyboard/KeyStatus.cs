using Microsoft.Xna.Framework.Input;

namespace Beryllium.MonoInput.Keyboard;

public enum KeyStates
{
    Pressed,
    Down,
    Up
}

public class KeyStatus
{
    public Keys Key { get; }

    private KeyStates _keyState;
    public KeyStates KeyState
    {
        get => _keyState;
        internal set
        {
            _keyState = value;
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