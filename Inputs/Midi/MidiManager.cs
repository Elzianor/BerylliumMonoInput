namespace BerylliumMonoInput.Inputs.Midi;

public static class MidiManager
{
    private static readonly ConcurrentQueue<Action> MainThreadQueue;
    private static readonly MidiWatcher Watcher;
    private static readonly MidiMessageHandler MessageHandler;
    private static readonly HashSet<int> CurrentNotes;

    public static bool Disposed { get; private set; }
    public static IReadOnlyList<MidiDeviceInfo> Devices => Watcher.Devices;
    public static MidiDeviceInfo SelectedDevice { get; private set; }
    public static readonly LinkedList<NoteStatus> ActiveNotes;

    #region Events
    public static event Action OnDevicesChanged;
    public static event Action<MidiDeviceInfo> OnDeviceSelected;
    #endregion

    static MidiManager()
    {
        MainThreadQueue = new ConcurrentQueue<Action>();

        Watcher = new MidiWatcher();
        Watcher.OnDevicesChanged += WatcherDevicesChangedHandler;
        Watcher.Start();

        MessageHandler = new MidiMessageHandler();
        MessageHandler.OnNotePressed += MessageHandlerNotePressedHandler;
        MessageHandler.OnNoteReleased += MessageHandlerNoteReleasedHandler;

        CurrentNotes = [];
        ActiveNotes = [];
    }

    public static async void TryConnectToMidiDevice(string deviceId)
    {
        if (Disposed) return;

        var selectedDeviceId = await MessageHandler.OpenMidiDeviceAsync(deviceId);

        SelectedDevice = Devices.FirstOrDefault(d => d.Id == selectedDeviceId);

        MainThreadQueue.Enqueue(() => OnDeviceSelected?.Invoke(SelectedDevice));
    }

    public static void Update()
    {
        if (Disposed) return;

        Watcher.Update();

        while (MainThreadQueue.TryDequeue(out var action)) action();

        UpdateActiveNotes();
    }

    public static void Dispose()
    {
        if (Disposed) return;

        SelectedDevice = null;
        OnDeviceSelected?.Invoke(SelectedDevice);

        Watcher.OnDevicesChanged -= WatcherDevicesChangedHandler;

        MessageHandler.OnNotePressed -= MessageHandlerNotePressedHandler;
        MessageHandler.OnNoteReleased -= MessageHandlerNoteReleasedHandler;

        OnDevicesChanged = null;

        MainThreadQueue.Clear();

        MessageHandler.Dispose();
        Watcher.Dispose();

        Disposed = true;
    }

    #region Event handlers
    private static void WatcherDevicesChangedHandler()
    {
        if (SelectedDevice != null &&
            Devices.All(d => d.Id != SelectedDevice.Id))
        {
            MessageHandler.CloseMidiDevice();
            SelectedDevice = null;
            OnDeviceSelected?.Invoke(SelectedDevice);
        }

        OnDevicesChanged?.Invoke();
    }

    private static void MessageHandlerNotePressedHandler(int note)
    {
        MainThreadQueue.Enqueue(() => CurrentNotes.Add(note));
    }

    private static void MessageHandlerNoteReleasedHandler(int note)
    {
        MainThreadQueue.Enqueue(() => CurrentNotes.Remove(note));
    }
    #endregion

    #region Helpers
    private static void UpdateActiveNotes()
    {
        foreach (var activeNote in ActiveNotes) activeNote.IsHandled = false;

        foreach (var currentNote in CurrentNotes)
        {
            var activeNote = ActiveNotes.FirstOrDefault(noteStatus => noteStatus.Note == currentNote);

            if (activeNote == null)
                ActiveNotes.AddLast(new NoteStatus(currentNote));
            else
                activeNote.State = ButtonStates.Down;

            activeNote?.IsHandled = true;
        }

        var currentActiveNote = ActiveNotes.First;

        while (currentActiveNote != null)
        {
            var nextActiveNote = currentActiveNote.Next;

            if (!currentActiveNote.Value.IsHandled)
            {
                if (currentActiveNote.Value.State != ButtonStates.Up)
                    currentActiveNote.Value.State = ButtonStates.Up;
                else
                    ActiveNotes.Remove(currentActiveNote);
            }

            currentActiveNote = nextActiveNote;
        }
    }
    #endregion
}
