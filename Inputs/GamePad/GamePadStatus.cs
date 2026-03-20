namespace BerylliumMonoInput.Inputs.GamePad;

public class GamePadStatus
{
    public PlayerIndex PlayerIndex { get; }
    public bool IsConnected
    {
        get;
        internal set
        {
            StatusChanged = StatusChanged ||
                            field != value;

            field = value;
        }
    }

    public GamePadButtonInfo A { get; } = new(GamePadButtonTypes.A);
    public GamePadButtonInfo B { get; } = new(GamePadButtonTypes.B);
    public GamePadButtonInfo X { get; } = new(GamePadButtonTypes.X);
    public GamePadButtonInfo Y { get; } = new(GamePadButtonTypes.Y);
    public GamePadButtonInfo Back { get; } = new(GamePadButtonTypes.Back);
    public GamePadButtonInfo Start { get; } = new(GamePadButtonTypes.Start);
    public GamePadButtonInfo Home { get; } = new(GamePadButtonTypes.Home);
    public GamePadButtonInfo LeftStick { get; } = new(GamePadButtonTypes.LeftStick);
    public GamePadButtonInfo RightStick { get; } = new(GamePadButtonTypes.RightStick);
    public GamePadButtonInfo LeftShoulder { get; } = new(GamePadButtonTypes.LeftShoulder);
    public GamePadButtonInfo RightShoulder { get; } = new(GamePadButtonTypes.RightShoulder);
    public GamePadButtonInfo Up { get; } = new(GamePadButtonTypes.Up);
    public GamePadButtonInfo Down { get; } = new(GamePadButtonTypes.Down);
    public GamePadButtonInfo Left { get; } = new(GamePadButtonTypes.Left);
    public GamePadButtonInfo Right { get; } = new(GamePadButtonTypes.Right);

    public Vector2 LeftStickShift { get; private set; }
    public Vector2 RightStickShift { get; private set; }

    public Vector2 LeftStickShiftDelta { get; private set; }
    public Vector2 RightStickShiftDelta { get; private set; }

    public float LeftTriggerValue { get; private set; }
    public float RightTriggerValue { get; private set; }

    public float LeftTriggerValueDelta { get; private set; }
    public float RightTriggerValueDelta { get; private set; }

    public bool IsAnyButtonActive =>
        A.Active ||
        B.Active ||
        X.Active ||
        Y.Active ||
        Back.Active ||
        Start.Active ||
        Home.Active ||
        LeftStick.Active ||
        RightStick.Active ||
        LeftShoulder.Active ||
        RightShoulder.Active ||
        Up.Active ||
        Down.Active ||
        Left.Active ||
        Right.Active;
    public bool IsAnyButtonPressed => IsAnyButtonInState(ButtonStates.Pressed);
    public bool IsAnyButtonDown => IsAnyButtonInState(ButtonStates.Down);
    public bool IsAnyButtonUp => IsAnyButtonInState(ButtonStates.Up);

    internal bool StatusChanged { get; set; }

    internal GamePadStatus(PlayerIndex playerIndex)
    {
        PlayerIndex = playerIndex;
    }

    internal void UpdateButtonsStates(GamePadState gamePadState)
    {
        StatusChanged = StatusChanged ||
                        A.SetState(gamePadState.Buttons.A == ButtonState.Pressed) ||
                        B.SetState(gamePadState.Buttons.B == ButtonState.Pressed) ||
                        X.SetState(gamePadState.Buttons.X == ButtonState.Pressed) ||
                        Y.SetState(gamePadState.Buttons.Y == ButtonState.Pressed) ||
                        Back.SetState(gamePadState.Buttons.Back == ButtonState.Pressed) ||
                        Start.SetState(gamePadState.Buttons.Start == ButtonState.Pressed) ||
                        Home.SetState(gamePadState.Buttons.BigButton == ButtonState.Pressed) ||
                        LeftStick.SetState(gamePadState.Buttons.LeftStick == ButtonState.Pressed) ||
                        RightStick.SetState(gamePadState.Buttons.RightStick == ButtonState.Pressed) ||
                        LeftShoulder.SetState(gamePadState.Buttons.LeftShoulder == ButtonState.Pressed) ||
                        RightShoulder.SetState(gamePadState.Buttons.RightShoulder == ButtonState.Pressed) ||
                        Up.SetState(gamePadState.DPad.Up == ButtonState.Pressed) ||
                        Down.SetState(gamePadState.DPad.Down == ButtonState.Pressed) ||
                        Left.SetState(gamePadState.DPad.Left == ButtonState.Pressed) ||
                        Right.SetState(gamePadState.DPad.Right == ButtonState.Pressed);
    }

    internal void UpdateStickShifts(GamePadState gamePadState)
    {
        var leftStickNewShift = gamePadState.ThumbSticks.Left;
        var rightStickNewShift = gamePadState.ThumbSticks.Right;

        LeftStickShiftDelta = leftStickNewShift - LeftStickShift;
        RightStickShiftDelta = rightStickNewShift - RightStickShift;

        StatusChanged = StatusChanged ||
                        LeftStickShiftDelta != Vector2.Zero ||
                        RightStickShiftDelta != Vector2.Zero;

        LeftStickShift = leftStickNewShift;
        RightStickShift = rightStickNewShift;
    }

    internal void UpdateTriggerValues(GamePadState gamePadState)
    {
        var leftTriggerNewValue = gamePadState.Triggers.Left;
        var rightTriggerNewValue = gamePadState.Triggers.Right;

        LeftTriggerValueDelta = leftTriggerNewValue - LeftTriggerValue;
        RightTriggerValueDelta = rightTriggerNewValue - RightTriggerValue;

        StatusChanged = StatusChanged ||
                        LeftTriggerValueDelta != 0 ||
                        RightTriggerValueDelta != 0;

        LeftTriggerValue = leftTriggerNewValue;
        RightTriggerValue = rightTriggerNewValue;
    }

    #region Helpers
    private bool IsAnyButtonInState(ButtonStates state)
    {
        return A.State == state ||
               B.State == state ||
               X.State == state ||
               Y.State == state ||
               Back.State == state ||
               Start.State == state ||
               Home.State == state ||
               LeftStick.State == state ||
               RightStick.State == state ||
               LeftShoulder.State == state ||
               RightShoulder.State == state ||
               Up.State == state ||
               Down.State == state ||
               Left.State == state ||
               Right.State == state;
    }
    #endregion
}
