using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BeatMapListLoader : MonoBehaviour
{
    public List<BeatMapDetails> beatMaps { get; set; } = new();
    
    public void LoadBeatMaps()
    {
        string[] beatMapsPaths = Directory.GetDirectories("Assets/_main/Resources/BeatMaps");
        foreach (var beatMapPath in beatMapsPaths)
        {
            var beatMapDetails = JsonUtility.FromJson<BeatMapDetails>(File.ReadAllText(beatMapPath + "/details.json"));
            beatMapDetails.path = beatMapPath;
            beatMaps.Add(beatMapDetails);
        }
    }
}