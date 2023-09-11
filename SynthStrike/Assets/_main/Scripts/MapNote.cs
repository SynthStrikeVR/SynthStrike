using System;
using UnityEngine;

[Serializable]
public class MapNote
{
    public long timestamp;
    [field: NonSerialized] public bool visible { get; set; }
    [field: NonSerialized] public bool destroyed { get; set; }
    [field: NonSerialized] public GameObject notePrefab { get; set; }

    public MapNote(long timestamp)
    {
        this.timestamp = timestamp;
    }
}