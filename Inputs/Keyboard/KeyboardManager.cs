using Microsoft.Xna.Framework.Input;

namespace Beryllium.MonoInput.Keyboard;

public static class KeyboardManager
{
    public static readonly LinkedList<KeyStatus> ActiveKeys;

    public static bool IsCtrlActive => IsKeyActive(Keys.LeftControl) || IsKeyActive(Keys.RightControl);
    public static bool IsShiftActive => IsKeyActive(Keys.LeftShift) || IsKeyActive(Keys.RightShift);
    public static bool IsAltActive => IsKeyActive(Keys.LeftAlt) || IsKeyActive(Keys.RightAlt);

    static KeyboardManager()
    {
        ActiveKeys = new LinkedList<KeyStatus>();
    }

    public static bool IsKeyPressed(Keys key)
    {
        return ActiveKeys.Any(keyStatus => keyStatus.Key == key && keyStatus.KeyState == KeyStates.Pressed);
    }

    public static bool IsKeyDown(Keys key)
    {
        return ActiveKeys.Any(keyStatus => keyStatus.Key == key && keyStatus.KeyState == KeyStates.Down);
    }

    public static bool IsKeyUp(Keys key)
    {
        return ActiveKeys.Any(keyStatus => keyStatus.Key == key && keyStatus.KeyState == KeyStates.Up);
    }

    public static bool IsKeyActive(Keys key)
    {
        return IsKeyPressed(key) || IsKeyDown(key);
    }

    public static void Update()
    {
        var newActiveKeys = Keyboard.GetState().GetPressedKeys();
        
        foreach (var activeKey in ActiveKeys)
        {
            activeKey.StateChanged = false;
        }

        foreach (var newActiveKey in newActiveKeys)
        {
            var currentActiveKey = ActiveKeys.FirstOrDefault(ak => ak.Key == newActiveKey);
            if (currentActiveKey == null) ActiveKeys.AddLast(new KeyStatus(newActiveKey));
            else currentActiveKey.KeyState = KeyStates.Down;
        }

        var currentKey = ActiveKeys.First;

        while (currentKey != null)
        {
            var nextKey = currentKey.Next;

            if (!currentKey.Value.StateChanged)
            {
                if (currentKey.Value.KeyState != KeyStates.Up)
                    currentKey.Value.KeyState = KeyStates.Up;
                else
                    ActiveKeys.Remove(currentKey);
            }

            currentKey = nextKey;
        }
    }
}