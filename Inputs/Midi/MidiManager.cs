namespace BerylliumMonoInput.Inputs.Midi;

public static class MidiManager
{
    private static readonly Lock Lock = new();

    private static HashSet<MidiDeviceInfo> _knownDevices = [];

    private static readonly HashSet<MidiDeviceInfo> CurrentDevices = [];
    private static readonly Dictionary<int, NoteStatus> CurrentNotes = [];
    private static readonly List<int> NotesToDelete = [];

    public static List<NoteStatus> ActiveNotes { get; private set; } = [];

    public static bool IsNotePressed(int note)
    {
        return ActiveNotes.Any(noteStatus => noteStatus.Note == note && noteStatus.State == ButtonStates.Pressed);
    }

    public static bool IsNoteDown(int note)
    {
        return ActiveNotes.Any(noteStatus => noteStatus.Note == note && noteStatus.State == ButtonStates.Down);
    }

    public static bool IsNoteUp(int note)
    {
        return ActiveNotes.Any(noteStatus => noteStatus.Note == note && noteStatus.State == ButtonStates.Up);
    }

    public static bool IsNoteActive(int note)
    {
        return IsNotePressed(note) || IsNoteDown(note) || IsNoteUp(note);
    }

    public static void Update()
    {
        // check if midi devices were connected/disconnected since previous Update() call
        UpdateCurrentDevices();

        // Detect connected
        foreach (var device in CurrentDevices.Except(_knownDevices)) device.MidiIn.MessageReceived += MidiInMessageReceivedHandler;

        // Detect disconnected
        foreach (var device in _knownDevices.Except(CurrentDevices)) device.MidiIn.MessageReceived -= MidiInMessageReceivedHandler;

        _knownDevices = CurrentDevices;

        // delete/mark Up notes
        lock (Lock)
        {
            foreach (var note in NotesToDelete) CurrentNotes.Remove(note);

            ActiveNotes = CurrentNotes.Values.ToList();

            NotesToDelete.Clear();

            foreach (var kvp in CurrentNotes)
                if (kvp.Value.State == ButtonStates.Up)
                    NotesToDelete.Add(kvp.Key);
        }
    }

    #region Event handlers
    private static void MidiInMessageReceivedHandler(object sender, MidiInMessageEventArgs e)
    {
        if (e.MidiEvent is not NoteEvent noteEvent) return;

        switch (noteEvent.CommandCode)
        {
            case MidiCommandCode.NoteOn:
                UpdateCurrentNote(noteEvent.NoteNumber, noteEvent.Velocity > 0);

                break;

            case MidiCommandCode.NoteOff:
                UpdateCurrentNote(noteEvent.NoteNumber, false);

                break;
        }
    }
    #endregion

    #region Helpers
    private static void UpdateCurrentDevices()
    {
        CurrentDevices.Clear();

        for (var i = 0; i < MidiIn.NumberOfDevices; i++)
        {
            var cap = MidiIn.DeviceInfo(i);
            CurrentDevices.Add(new MidiDeviceInfo(i, cap.ProductName, new MidiIn(i)));
        }
    }

    private static void UpdateCurrentNote(int note, bool isOn)
    {
        lock (Lock)
        {
            if (CurrentNotes.TryGetValue(note, out var status))
                status.State = isOn ? ButtonStates.Down : ButtonStates.Up;
            else if (isOn) CurrentNotes.Add(note, new NoteStatus(note));
        }
    }
    #endregion
}
