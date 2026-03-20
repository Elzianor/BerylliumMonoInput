namespace BerylliumMonoInput.Inputs.Mouse;

public class MouseButtonsStates
{
    public ButtonStates LeftButtonState { get; } = MouseManager.MouseStatus.LeftButton.State;
    public ButtonStates MiddleButtonState { get; } = MouseManager.MouseStatus.MiddleButton.State;
    public ButtonStates RightButtonState { get; } = MouseManager.MouseStatus.RightButton.State;
    public ButtonStates XButton1State { get; } = MouseManager.MouseStatus.XButton1.State;
    public ButtonStates XButton2State { get; } = MouseManager.MouseStatus.XButton2.State;

    public static bool operator ==(MouseButtonsStates first, MouseButtonsStates second)
    {
        if (ReferenceEquals(first, second)) return true;

        if (first is null ||
            second is null)
            return false;

        return first.LeftButtonState == second.LeftButtonState &&
               first.MiddleButtonState == second.MiddleButtonState &&
               first.RightButtonState == second.RightButtonState &&
               first.XButton1State == second.XButton1State &&
               first.XButton2State == second.XButton2State;
    }

    public static bool operator !=(MouseButtonsStates first, MouseButtonsStates second)
    {
        return !(first == second);
    }

    public override bool Equals(object obj)
    {
        if (obj is MouseButtonsStates other) return this == other;

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
