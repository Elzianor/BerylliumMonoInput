using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Beryllium.UI.Inputs;

public class MouseStatus
{
    public ButtonInfo LeftButton { get; } = new(ButtonTypes.Left);
    public ButtonInfo MiddleButton { get; } = new(ButtonTypes.Middle);
    public ButtonInfo RightButton { get; } = new(ButtonTypes.Right);
    public ButtonInfo XButton1 { get; } = new(ButtonTypes.XButton1);
    public ButtonInfo XButton2 { get; } = new(ButtonTypes.XButton2);
    private Point _position;
    public Point Position
    {
        get => _position;
        private set
        {
            if (_position == value) return;
            _position = value;
            MouseManager.TriggerOnPositionChanged(value);
        }
    }
    public int WheelCumulativeValue { get; private set; }
    public int HorizontalWheelCumulativeValue { get; private set; }
    public int DeltaX { get; private set; }
    public int DeltaY { get; private set; }
    private int _wheelDelta;
    public int WheelDelta
    {
        get => _wheelDelta;
        private set
        {
            if (_wheelDelta == value) return;
            _wheelDelta = value;
            MouseManager.TriggerOnWheelDeltaChanged(value);
        }
    }
    private int _horizontalWheelDelta;
    public int HorizontalWheelDelta
    {
        get => _horizontalWheelDelta;
        private set
        {
            if (_horizontalWheelDelta == value) return;
            _horizontalWheelDelta = value;
            MouseManager.TriggerOnHorizontalWheelDeltaChanged(value);
        }
    }
    public bool IsAnyButtonActive => LeftButton.Active ||
                                     MiddleButton.Active ||
                                     RightButton.Active ||
                                     XButton1.Active ||
                                     XButton2.Active;
    public bool IsAnyButtonPressed => IsAnyButtonInState(ButtonStates.Pressed);
    public bool IsAnyButtonDown => IsAnyButtonInState(ButtonStates.Down);
    public bool IsAnyButtonUp => IsAnyButtonInState(ButtonStates.Up);

    internal void UpdatePositionData(MouseState mouseState)
    {
        DeltaX = mouseState.X - Position.X;
        DeltaY = mouseState.Y - Position.Y;

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

    private bool IsAnyButtonInState(ButtonStates state)
    {
        return LeftButton.State == state ||
               MiddleButton.State == state ||
               RightButton.State == state ||
               XButton1.State == state ||
               XButton2.State == state;
    }
}