namespace BerylliumMonoInput.Inputs.Midi;

public class NoteStatus(int note)
{
    public int Note { get; } = note;
    public ButtonStates State { get; internal set; } = ButtonStates.Pressed;
    internal bool IsHandled { get; set; } = true;
}
