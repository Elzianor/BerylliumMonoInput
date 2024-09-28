using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Beryllium.UI.Inputs;

public enum ButtonTypes
{
    Left,
    Middle,
    Right,
    XButton1,
    XButton2
}

public enum ButtonStates
{
    None,
    Pressed,
    Down,
    Up
}

public class ButtonsStates
{
    public ButtonStates LeftButtonState { get; } = MouseManager.MouseStatus.LeftButton.State;
    public ButtonStates MiddleButtonState { get; } = MouseManager.MouseStatus.MiddleButton.State;
    public ButtonStates RightButtonState { get; } = MouseManager.MouseStatus.RightButton.State;
    public ButtonStates XButton1State { get; } = MouseManager.MouseStatus.XButton1.State;
    public ButtonStates XButton2State { get; } = MouseManager.MouseStatus.XButton2.State;

    public static bool operator ==(ButtonsStates first, ButtonsStates second)
    {
        if (ReferenceEquals(first, second)) return true;

        if (first is null || second is null) return false;

        return first.LeftButtonState == second.LeftButtonState &&
               first.MiddleButtonState == second.MiddleButtonState &&
               first.RightButtonState == second.RightButtonState &&
               first.XButton1State == second.XButton1State &&
               first.XButton2State == second.XButton2State;
    }

    public static bool operator !=(ButtonsStates first, ButtonsStates second)
    {
        return !(first == second);
    }

    public override bool Equals(object obj)
    {
        if (obj is ButtonsStates other)
            return this == other;

        return false;
    }

    public override int GetHashCode()
    {
        // Start with a prime number
        var hash = 17;

        hash ^= LeftButtonState.GetHashCode();
        hash ^= MiddleButtonState.GetHashCode();
        hash ^= RightButtonState.GetHashCode();
        hash ^= XButton1State.GetHashCode();
        hash ^= XButton2State.GetHashCode();

        return hash;
    }

    public override string ToString()
    {
        return $"L: {LeftButtonState} M: {MiddleButtonState} R: {RightButtonState} X1: {XButton1State} X2: {XButton2State}";
    }
}

public static class MouseManager
{
    public static MouseStatus MouseStatus { get; }

    public delegate void ButtonsStatesChanged(ButtonsStates buttonsStates);
    public static event ButtonsStatesChanged OnButtonsStatesChanged;

    public delegate void PositionChanged(Point newPosition);
    public static event PositionChanged OnPositionChanged;

    public delegate void WheelDeltaChanged(int delta);
    public static event WheelDeltaChanged OnWheelDeltaChanged;

    public delegate void HorizontalWheelDeltaChanged(int delta);
    public static event HorizontalWheelDeltaChanged OnHorizontalWheelDeltaChanged;

    static MouseManager()
    {
        MouseStatus = new MouseStatus();
    }

    public static void Update()
    {
        var mouseState = Mouse.GetState();

        MouseStatus.UpdatePositionData(mouseState);
        MouseStatus.UpdateWheelsData(mouseState);
        MouseStatus.UpdateButtonsStates(mouseState);
    }

    public static void UpdatePositionData()
    {
        MouseStatus.UpdatePositionData(Mouse.GetState());
    }

    public static void UpdateWheelsData()
    {
        MouseStatus.UpdateWheelsData(Mouse.GetState());
    }
    
    public static void UpdateButtonsStates()
    {
        MouseStatus.UpdateButtonsStates(Mouse.GetState());
    }

    public static void TriggerOnButtonsStatesChanged() => OnButtonsStatesChanged?.Invoke(new ButtonsStates());
    public static void TriggerOnPositionChanged(Point newPosition) => OnPositionChanged?.Invoke(newPosition);
    public static void TriggerOnWheelDeltaChanged(int delta) => OnWheelDeltaChanged?.Invoke(delta);
    public static void TriggerOnHorizontalWheelDeltaChanged(int delta) => OnHorizontalWheelDeltaChanged?.Invoke(delta);
}