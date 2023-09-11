using System;
using System.Collections.Generic;

[Serializable]
public class BeatMapDetails
{
    public string title;
    public string author;
    public string beatMapAuthor;
    public List<string> difficulties = new();
    [field: NonSerialized] public string path { get; set; }
}