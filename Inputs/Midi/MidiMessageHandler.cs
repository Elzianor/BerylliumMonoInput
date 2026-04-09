namespace BerylliumMonoInput.Inputs.Midi;

internal sealed class MidiMessageHandler : IDisposable
{
    private MidiInPort _midiPort;

    #region Events
    public event Action<int> OnNotePressed;
    public event Action<int> OnNoteReleased;
    #endregion

    public async Task<string> OpenMidiDeviceAsync(string deviceId)
    {
        try
        {
            CloseMidiDevice();

            _midiPort = await MidiInPort.FromIdAsync(deviceId);

            if (_midiPort == null) return null;

            _midiPort.MessageReceived += MidiPortMessageReceivedHandler;

            return deviceId;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public void CloseMidiDevice()
    {
        if (_midiPort == null) return;

        _midiPort.MessageReceived -= MidiPortMessageReceivedHandler;
        _midiPort.Dispose();
    }

    public void Dispose()
    {
        CloseMidiDevice();

        OnNotePressed = null;
        OnNoteReleased = null;
    }

    #region Event handlers
    private void MidiPortMessageReceivedHandler(MidiInPort sender, MidiMessageReceivedEventArgs args)
    {
        switch (args.Message)
        {
            case MidiNoteOnMessage { Velocity: > 0 } noteMsg:
                OnNotePressed?.Invoke(noteMsg.Note);

                break;
            case MidiNoteOnMessage noteMsg:
                OnNoteReleased?.Invoke(noteMsg.Note);

                break;
            case MidiNoteOffMessage noteMsg:
                OnNoteReleased?.Invoke(noteMsg.Note);

                break;
        }
    }
    #endregion
}
