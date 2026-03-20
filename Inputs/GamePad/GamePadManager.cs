namespace BerylliumMonoInput.Inputs.GamePad;

public static class GamePadManager
{
    public static GamePadStatus[] GamePadsStatus { get; }

    #region Events
    public delegate void StatusChanged(PlayerIndex playerIndex);
    public static event StatusChanged OnStatusChanged;
    #endregion

    static GamePadManager()
    {
        GamePadsStatus = new GamePadStatus[4];

        for (var i = 0; i < 4; i++) GamePadsStatus[i] = new GamePadStatus((PlayerIndex)i);
    }

    public static void SetVibration(PlayerIndex index, float lowFreqMotor, float highFreqMotor)
    {
        Microsoft.Xna.Framework.Input.GamePad.SetVibration(index, lowFreqMotor, highFreqMotor);
    }

    #region Updaters
    public static void Update()
    {
        for (var i = 0; i < 4; i++)
        {
            var gamePadState = Microsoft.Xna.Framework.Input.GamePad.GetState(i, GamePadDeadZone.Circular);
            GamePadsStatus[i].IsConnected = gamePadState.IsConnected;

            if (!gamePadState.IsConnected)
            {
                TryReportStatusChanged(i);

                continue;
            }

            GamePadsStatus[i].UpdateButtonsStates(gamePadState);
            GamePadsStatus[i].UpdateStickShifts(gamePadState);
            GamePadsStatus[i].UpdateTriggerValues(gamePadState);

            TryReportStatusChanged(i);
        }
    }
    #endregion

    #region Helpers
    private static void TryReportStatusChanged(int playerIndex)
    {
        if (!GamePadsStatus[playerIndex].StatusChanged) return;

        GamePadsStatus[playerIndex].StatusChanged = false;
        OnStatusChanged?.Invoke((PlayerIndex)playerIndex);
    }
    #endregion
}
