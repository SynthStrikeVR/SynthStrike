using System;

[Serializable]
public class MapNote
{
    public ulong timestamp { get; set; }
    public bool visible { get; set; } = false;

    public MapNote(ulong timestamp)
    {
        this.timestamp = timestamp;
    }
}