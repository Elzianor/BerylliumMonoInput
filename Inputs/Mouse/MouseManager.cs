namespace BerylliumMonoInput.Inputs.Mouse;

public static class MouseManager
{
    public static MouseStatus MouseStatus { get; }

    #region Events
    public delegate void ButtonsStatesChanged(MouseButtonsStates mouseButtonsStates);
    public static event ButtonsStatesChanged OnButtonsStatesChanged;

    public delegate void PositionChanged(Point newPosition);
    public static event PositionChanged OnPositionChanged;

    public delegate void WheelDeltaChanged(int delta);
    public static event WheelDeltaChanged OnWheelDeltaChanged;

    public delegate void HorizontalWheelDeltaChanged(int delta);
    public static event HorizontalWheelDeltaChanged OnHorizontalWheelDeltaChanged;
    #endregion

    static MouseManager()
    {
        MouseStatus = new MouseStatus();
    }

    #region Updaters
    public static void Update()
    {
        var mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

        MouseStatus.UpdatePositionData(mouseState);
        MouseStatus.UpdateWheelsData(mouseState);
        MouseStatus.UpdateButtonsStates(mouseState);
    }

    public static void UpdatePositionData()
    {
        MouseStatus.UpdatePositionData(Microsoft.Xna.Framework.Input.Mouse.GetState());
    }

    public static void UpdateWheelsData()
    {
        MouseStatus.UpdateWheelsData(Microsoft.Xna.Framework.Input.Mouse.GetState());
    }

    public static void UpdateButtonsStates()
    {
        MouseStatus.UpdateButtonsStates(Microsoft.Xna.Framework.Input.Mouse.GetState());
    }
    #endregion

    public static MouseButtonsStates GetButtonsStates()
    {
        return new MouseButtonsStates();
    }

    #region Event invokers
    internal static void TriggerOnButtonsStatesChanged()
    {
        OnButtonsStatesChanged?.Invoke(new MouseButtonsStates());
    }

    internal static void TriggerOnPositionChanged(Point newPosition)
    {
        OnPositionChanged?.Invoke(newPosition);
    }

    internal static void TriggerOnWheelDeltaChanged(int delta)
    {
        OnWheelDeltaChanged?.Invoke(delta);
    }

    internal static void TriggerOnHorizontalWheelDeltaChanged(int delta)
    {
        OnHorizontalWheelDeltaChanged?.Invoke(delta);
    }
    #endregion
}
