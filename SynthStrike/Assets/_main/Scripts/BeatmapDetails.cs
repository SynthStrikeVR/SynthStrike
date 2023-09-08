using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class BeatmapDetails
{
    private string title { get; set; }
    private string author { get; set; }
    private string beatmapAuthor { get; set; }
    [field: NonSerialized] private string path { get; set; }
}
