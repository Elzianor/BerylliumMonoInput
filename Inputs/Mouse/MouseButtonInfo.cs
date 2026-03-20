namespace BerylliumMonoInput.Inputs.Mouse;

public class MouseButtonInfo
{
    public MouseButtonTypes Type { get; }
    public ButtonStates State { get; private set; }

    public bool Active => State != ButtonStates.Idle;
    public bool Idle => !Active;
    public bool Pressed => State == ButtonStates.Pressed;
    public bool Down => State == ButtonStates.Down;
    public bool Up => State == ButtonStates.Up;

    public MouseButtonInfo(MouseButtonTypes type)
    {
        Type = type;
    }

    internal void SetState(bool isDown)
    {
        var oldState = State;

        switch (State)
        {
            case ButtonStates.Idle:
                if (isDown) State = ButtonStates.Pressed;

                break;
            case ButtonStates.Pressed:
                State = isDown ? ButtonStates.Down : ButtonStates.Up;

                break;
            case ButtonStates.Down:
                if (!isDown) State = ButtonStates.Up;

                break;
            case ButtonStates.Up:
                State = isDown ? ButtonStates.Pressed : ButtonStates.Idle;

                break;
        }

        if (oldState != State) MouseManager.TriggerOnButtonsStatesChanged();
    }

    public override string ToString()
    {
        return $"{Type} | {State}";
    }
}
