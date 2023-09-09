using System;

[Serializable]
public class BeatMapDetails
{
    private string title { get; set; }
    private string author { get; set; }
    private string beatMapAuthor { get; set; }
    [field: NonSerialized] private string path { get; set; }
}