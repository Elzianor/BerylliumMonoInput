namespace BerylliumMonoInput.Inputs.Mouse;

public class MouseStatus
{
    public MouseButtonInfo LeftButton { get; } = new(MouseButtonTypes.Left);
    public MouseButtonInfo MiddleButton { get; } = new(MouseButtonTypes.Middle);
    public MouseButtonInfo RightButton { get; } = new(MouseButtonTypes.Right);
    public MouseButtonInfo XButton1 { get; } = new(MouseButtonTypes.XButton1);
    public MouseButtonInfo XButton2 { get; } = new(MouseButtonTypes.XButton2);

    public Point Position
    {
        get;
        private set
        {
            if (field == value) return;

            field = value;
            MouseManager.TriggerOnPositionChanged(value);
        }
    }

    private Point _positionDelta;
    public Point PositionDelta => _positionDelta;

    public int WheelCumulativeValue { get; private set; }
    public int HorizontalWheelCumulativeValue { get; private set; }

    public int WheelDelta
    {
        get;
        private set
        {
            if (field == value) return;

            field = value;
            MouseManager.TriggerOnWheelDeltaChanged(value);
        }
    }

    public int HorizontalWheelDelta
    {
        get;
        private set
        {
            if (field == value) return;

            field = value;
            MouseManager.TriggerOnHorizontalWheelDeltaChanged(value);
        }
    }

    public bool IsAnyButtonActive =>
        LeftButton.Active ||
        MiddleButton.Active ||
        RightButton.Active ||
        XButton1.Active ||
        XButton2.Active;
    public bool IsAnyButtonPressed => IsAnyButtonInState(ButtonStates.Pressed);
    public bool IsAnyButtonDown => IsAnyButtonInState(ButtonStates.Down);
    public bool IsAnyButtonUp => IsAnyButtonInState(ButtonStates.Up);

    #region Updaters
    internal void UpdatePositionData(MouseState mouseState)
    {
        _positionDelta.X = mouseState.X - Position.X;
        _positionDelta.Y = mouseState.Y - Position.Y;

        Position = mouseState.Position;
    }

    internal void UpdateWheelsData(MouseState mouseState)
    {
        WheelDelta = mouseState.ScrollWheelValue - WheelCumulativeValue;
        WheelCumulativeValue = mouseState.ScrollWheelValue;

        HorizontalWheelDelta = mouseState.HorizontalScrollWheelValue - HorizontalWheelCumulativeValue;
        HorizontalWheelCumulativeValue = mouseState.HorizontalScrollWheelValue;
    }

    internal void UpdateButtonsStates(MouseState mouseState)
    {
        LeftButton.SetState(mouseState.LeftButton == ButtonState.Pressed);
        MiddleButton.SetState(mouseState.MiddleButton == ButtonState.Pressed);
        RightButton.SetState(mouseState.RightButton == ButtonState.Pressed);
        XButton1.SetState(mouseState.XButton1 == ButtonState.Pressed);
        XButton2.SetState(mouseState.XButton2 == ButtonState.Pressed);
    }
    #endregion

    #region Helpers
    private bool IsAnyButtonInState(ButtonStates state)
    {
        return LeftButton.State == state ||
               MiddleButton.State == state ||
               RightButton.State == state ||
               XButton1.State == state ||
               XButton2.State == state;
    }
    #endregion
}
