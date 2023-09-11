using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BeatMapListLoader : MonoBehaviour
{
    private List<BeatMapDetails> beatMaps { get; set; } = new();

    private void Start()
    {
        string[] beatMapsPaths = Directory.GetDirectories("Assets/_main/Resources/BeatMaps");
        foreach (var beatMapPath in beatMapsPaths)
        {
            Debug.Log(beatMapPath);
            var beatMapDetails = JsonUtility.FromJson<BeatMapDetails>(File.ReadAllText(beatMapPath + "/details.json"));
            beatMapDetails.path = beatMapPath;
            beatMaps.Add(beatMapDetails);
            Debug.Log(beatMapDetails.title);
        }
    }
}