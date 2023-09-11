using System;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    public bool isPlaying { get; set; }
    [SerializeField] private GameObject mapNotePrefab;
    private BeatMap currentlyPlayingBeatMap { get; set; }
    private AudioSource _audioSource;
    private AudioClip _audioClip;
    private long _currentTime;
    private float _playbackDelay;

    public Gameplay(BeatMap currentlyPlayingBeatMap)
    {
        this.currentlyPlayingBeatMap = currentlyPlayingBeatMap;
    }

    void Initialize(BeatMap beatMap, BeatMapDetails details)
    {
        currentlyPlayingBeatMap = beatMap;
        _audioSource = GetComponent<AudioSource>();
        _audioClip = Resources.Load<AudioClip>("BeatMaps/Touhou - Bad Apple!!/track");
        _audioClip.LoadAudioData();
        _audioSource.clip = _audioClip;
        _playbackDelay = 5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize(null, null);
        currentlyPlayingBeatMap = new BeatMap
        {
            beatMapNotes = new List<List<MapNote>>()
            {
                new() { new MapNote(1333) }, new() { new MapNote(1767) }, new(), new() { new MapNote(2202) },
                new() { new MapNote(2637) }, new(), new(), new(), new()
            }
        };
        _currentTime = 0;
    }
    
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_playbackDelay > 0)
        {
            _playbackDelay -= Time.deltaTime;
        }
        else if(!_audioSource.isPlaying)
        {
            _audioSource.Play();
            _playbackDelay = 0;
        }

        _currentTime = (long)((_audioSource.time - _playbackDelay) * 1000);
        if (currentlyPlayingBeatMap == null) return;
        for (var notesTrackNo = 0; notesTrackNo < 9; notesTrackNo++)
        {
            foreach (var mapNote in currentlyPlayingBeatMap.beatMapNotes[notesTrackNo])
            {
                if (!mapNote.visible && mapNote.timestamp - _currentTime < 4000)
                {
                    mapNote.notePrefab = Instantiate(mapNotePrefab,
                        transform.position + GenerateNotePosition(notesTrackNo, mapNote.timestamp),
                        GenerateNoteRotation(notesTrackNo));
                    mapNote.visible = true;
                }

                if (mapNote.visible && !mapNote.destroyed)
                {
                    var mapNotePosition = mapNote.notePrefab.transform.position;
                    mapNote.notePrefab.transform.position = new Vector3(mapNotePosition.x, mapNotePosition.y,
                        transform.position.z +
                        ((mapNote.timestamp - _currentTime) / 200.0f));
                }

                if (!mapNote.destroyed && mapNote.timestamp - _currentTime < -300)
                {
                    Debug.Log(mapNote.timestamp);
                    Destroy(mapNote.notePrefab);
                    mapNote.destroyed = true;
                }
            }
        }
    }

    private Vector3 GenerateNotePosition(int trackNo, long noteTimestamp)
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
        return new Vector3(relativeFramePosition.x, relativeFramePosition.y, (noteTimestamp - _currentTime) / 200.0f);
    }

    private Quaternion GenerateNoteRotation(int trackNo)
    {
        return trackNo is >= 3 and <= 5 ? new Quaternion() : new Quaternion(90.0f, 90.0f, 0.0f, 0.0f);
    }
}