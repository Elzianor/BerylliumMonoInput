namespace BerylliumMonoInput.Inputs.Midi;

internal sealed class MidiWatcher : IDisposable
{
    private readonly DeviceWatcher _inputWatcher;
    private readonly ConcurrentQueue<Action> _eventQueue = new();
    private readonly Dictionary<string, MidiDeviceInfo> _devices = [];

    public IReadOnlyList<MidiDeviceInfo> Devices => _devices.Values.ToList();

    #region Events
    public event Action OnDevicesChanged;
    #endregion

    public MidiWatcher()
    {
        _inputWatcher = DeviceInformation.CreateWatcher(MidiInPort.GetDeviceSelector());

        _inputWatcher.Added += OnAdded;
        _inputWatcher.Removed += OnRemoved;
    }

    public void Start()
    {
        _inputWatcher.Start();
    }

    public void Stop()
    {
        _inputWatcher.Stop();
    }

    public void Update()
    {
        while (_eventQueue.TryDequeue(out var action)) action();
    }

    public void Dispose()
    {
        Stop();

        _inputWatcher.Added -= OnAdded;
        _inputWatcher.Removed -= OnRemoved;

        OnDevicesChanged = null;
    }

    #region Event handlers
    private void OnAdded(DeviceWatcher sender, DeviceInformation args)
    {
        _eventQueue.Enqueue(() =>
        {
            _devices[args.Id] = new MidiDeviceInfo(args.Id, args.Name);
            OnDevicesChanged?.Invoke();
        });
    }

    private void OnRemoved(DeviceWatcher sender, DeviceInformationUpdate args)
    {
        _eventQueue.Enqueue(() =>
        {
            if (_devices.Remove(args.Id)) OnDevicesChanged?.Invoke();
        });
    }
    #endregion
}
