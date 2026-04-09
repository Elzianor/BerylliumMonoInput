namespace BerylliumMonoInput.Inputs.Midi;

public class MidiDeviceInfo(string id, string name)
{
    public string Id { get; } = id;
    public string Name { get; } = name;

    public override string ToString()
    {
        return Name;
    }
}
