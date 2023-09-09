using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private GameObject mapNotePrefab;
    private BeatMap currentlyPlayingBeatMap { get; set; }
    private long _currentTime = 0;

    public Gameplay(BeatMap currentlyPlayingBeatMap)
    {
        this.currentlyPlayingBeatMap = currentlyPlayingBeatMap;
    }

    void Initialize(BeatMap beatMap)
    {
        currentlyPlayingBeatMap = beatMap;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentlyPlayingBeatMap = new BeatMap
        {
            beatMapNotes = new List<List<MapNote>>()
            {
                new() { new MapNote(0) }, new() { new MapNote(0) }, new(), new() { new MapNote(0) },
                new() { new MapNote(100) }, new(), new(), new(), new()
            }
        };
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (currentlyPlayingBeatMap == null) return;
        for (var notesTrackNo = 0; notesTrackNo < 9; notesTrackNo++)
        {
            foreach (var mapNote in currentlyPlayingBeatMap.beatMapNotes[notesTrackNo])
            {
                if (!mapNote.visible)
                {
                    Instantiate(mapNotePrefab,
                        transform.position + GenerateNotePosition(notesTrackNo, mapNote.timestamp),
                        GenerateNoteRotation(notesTrackNo));
                    mapNote.visible = true;
                }
            }
        }
    }

    private Vector3 GenerateNotePosition(int trackNo, ulong noteTimestamp)
    {
        Vector2 relativeFramePosition = trackNo switch
        {
            0 => new(-0.5f, 0.3f),
            1 => new(-0.5f, 0.0f),
            2 => new(-0.5f, -0.3f),
            3 => new(-0.3f, -0.5f),
            4 => new(0.0f, -0.5f),
            5 => new(0.3f, -0.5f),
            6 => new(0.5f, -0.3f),
            7 => new(0.5f, 0.0f),
            8 => new(0.5f, 0.3f),
            _ => new()
        };
        return new Vector3(relativeFramePosition.x, relativeFramePosition.y, noteTimestamp / 200.0f);
    }

    private Quaternion GenerateNoteRotation(int trackNo)
    {
        return trackNo is >= 3 and <= 5 ? new Quaternion() : new Quaternion(90.0f, 90.0f, 0.0f, 0.0f);
    }
}