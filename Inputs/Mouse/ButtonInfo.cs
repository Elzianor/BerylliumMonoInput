namespace Beryllium.MonoInput.MouseInput;

public class ButtonInfo
{
    public ButtonTypes Type { get; }
    public ButtonStates State { get; private set; }

    public bool Active => State != ButtonStates.None;
    public bool Idle => !Active;
    public bool Pressed => State == ButtonStates.Pressed;
    public bool Down => State == ButtonStates.Down;
    public bool Up => State == ButtonStates.Up;

    public ButtonInfo(ButtonTypes type)
    {
        Type = type;
    }

    internal void SetState(bool isDown)
    {
        var oldState = State;

        switch (State)
        {
            case ButtonStates.None:
                if (isDown) State = ButtonStates.Pressed;

                break;
            case ButtonStates.Pressed:
                State = isDown ? ButtonStates.Down : ButtonStates.Up;

                break;
            case ButtonStates.Down:
                if (!isDown) State = ButtonStates.Up;

                break;
            case ButtonStates.Up:
                State = isDown ? ButtonStates.Pressed : ButtonStates.None;

                break;
        }

        if (oldState != State) MouseManager.TriggerOnButtonsStatesChanged();
    }

    public override string ToString()
    {
        return $"{Type} | {State}";
    }
}
