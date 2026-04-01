namespace BerylliumMonoInput.Inputs.Keyboard;

public static class KeyboardManager
{
    public static readonly LinkedList<KeyStatus> ActiveKeys = [];

    public static bool IsCtrlActive => IsKeyActive(Keys.LeftControl) || IsKeyActive(Keys.RightControl);
    public static bool IsShiftActive => IsKeyActive(Keys.LeftShift) || IsKeyActive(Keys.RightShift);
    public static bool IsAltActive => IsKeyActive(Keys.LeftAlt) || IsKeyActive(Keys.RightAlt);

    public static bool IsKeyPressed(Keys key)
    {
        return ActiveKeys.Any(keyStatus => keyStatus.Key == key && keyStatus.State == ButtonStates.Pressed);
    }

    public static bool IsKeyDown(Keys key)
    {
        return ActiveKeys.Any(keyStatus => keyStatus.Key == key && keyStatus.State == ButtonStates.Down);
    }

    public static bool IsKeyUp(Keys key)
    {
        return ActiveKeys.Any(keyStatus => keyStatus.Key == key && keyStatus.State == ButtonStates.Up);
    }

    public static bool IsKeyActive(Keys key)
    {
        return IsKeyPressed(key) || IsKeyDown(key) || IsKeyUp(key);
    }

    #region Updaters
    public static void Update()
    {
        var currentKeys = Microsoft.Xna.Framework.Input.Keyboard.GetState().GetPressedKeys();

        foreach (var activeKey in ActiveKeys) activeKey.IsHandled = false;

        foreach (var currentKey in currentKeys)
        {
            var activeKey = ActiveKeys.FirstOrDefault(keyStatus => keyStatus.Key == currentKey);

            if (activeKey == null)
                ActiveKeys.AddLast(new KeyStatus(currentKey));
            else
                activeKey.State = ButtonStates.Down;

            activeKey?.IsHandled = true;
        }

        var currentActiveKey = ActiveKeys.First;

        while (currentActiveKey != null)
        {
            var nextActiveKey = currentActiveKey.Next;

            if (!currentActiveKey.Value.IsHandled)
            {
                if (currentActiveKey.Value.State != ButtonStates.Up)
                    currentActiveKey.Value.State = ButtonStates.Up;
                else
                    ActiveKeys.Remove(currentActiveKey);
            }

            currentActiveKey = nextActiveKey;
        }
    }
    #endregion
}
