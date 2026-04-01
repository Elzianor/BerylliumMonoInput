namespace BerylliumMonoInput.Inputs.Midi;

internal class MidiDeviceInfo : IEquatable<MidiDeviceInfo>
{
    private readonly int _hashCode;
    private readonly int _index;
    private readonly string _name;

    public MidiIn MidiIn { get; }

    public MidiDeviceInfo(int index, string name, MidiIn midiIn)
    {
        _hashCode = HashCode.Combine(index, name);

        _index = index;
        _name = name;
        MidiIn = midiIn;
    }

    public bool Equals(MidiDeviceInfo other)
    {
        if (other is null) return false;

        return _index == other._index && _name == other._name;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as MidiDeviceInfo);
    }

    public override int GetHashCode()
    {
        return _hashCode;
    }

    public override string ToString()
    {
        return $"[{_index}] '{_name}'";
    }
}
