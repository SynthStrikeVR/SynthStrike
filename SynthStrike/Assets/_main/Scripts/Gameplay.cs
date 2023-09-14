using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
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
    private int _combo = 0;
    private int _score = 0;
    [SerializeField] private TMP_Text _comboLabel;
    [SerializeField] private TMP_Text _scoreLabel;

    public Gameplay(BeatMap currentlyPlayingBeatMap)
    {
        this.currentlyPlayingBeatMap = currentlyPlayingBeatMap;
    }

    public void Initialize(BeatMapDetails details, string difficulty)
    {
        Debug.Log(details.path);
        currentlyPlayingBeatMap =  JsonUtility.FromJson<BeatMap>(
            File.ReadAllText(  details.path + "/"+ difficulty +".json"));
        _audioSource = GetComponent<AudioSource>();
        _audioClip = Resources.Load<AudioClip>(details.path.Remove(0, 23) + "/track");
        _audioClip.LoadAudioData();
        _audioSource.clip = _audioClip;
        _playbackDelay = 5f;
        isPlaying = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentTime = 0;
    }

    public void ClickAButton(int buttonNo)
    {
        Debug.Log($"Button ${buttonNo} has been clicked");
        var track = currentlyPlayingBeatMap.GetTrack(buttonNo);
        for (var i = 0; i < track.Count; i++)
        {
            var mapNote = track[i];
            if (mapNote.timestamp < _currentTime) continue;
            var notePrefab = mapNote.notePrefab;
            CalculateScore(mapNote);
            track.Remove(mapNote);
            Destroy(notePrefab);
            break;
        }
    }

    private void CalculateScore(MapNote note)
    {
        var timingDelta = Math.Abs(_currentTime - note.timestamp);
        switch (timingDelta)
        {
            case <= 1000 and > 300:
                _combo = 0;
                break;
            case <= 300 and > 100:
                _combo++;
                _score += 25;
                break;
            case <= 100 and > 50:
                _combo++;
                _score += 50;
                break;
            case <= 50:
                _combo++;
                _score += 100;
                break;
        }

        _comboLabel.text = _combo.ToString();
        _scoreLabel.text = _score.ToString();
    }

    private void BreakCombo()
    {
        _combo = 0;
        _comboLabel.text = _combo.ToString();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_audioSource == null)
        {
            return;
        }
        
        if (isPlaying)
        {
            if (_playbackDelay > 0)
            {
                _playbackDelay -= Time.deltaTime;
            }
            else if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
                _playbackDelay = 0;
            }
        }
        else
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Pause();
            }
        }


        _currentTime = (long)((_audioSource.time - _playbackDelay) * 1000);
        if (currentlyPlayingBeatMap == null) return;
        for (var notesTrackNo = 0; notesTrackNo < 9; notesTrackNo++)
        {
            foreach (var mapNote in currentlyPlayingBeatMap.GetTrack(notesTrackNo))
            {
                if (!mapNote.visible && mapNote.timestamp - _currentTime < 5000)
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
                        ((mapNote.timestamp - _currentTime) / 100.0f));
                }

                if (!mapNote.destroyed && mapNote.timestamp - _currentTime < -300)
                {
                    BreakCombo();
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
        return new Vector3(relativeFramePosition.x, relativeFramePosition.y, (noteTimestamp - _currentTime) / 100.0f);
    }

    private Quaternion GenerateNoteRotation(int trackNo)
    {
        return trackNo is >= 3 and <= 5 ? new Quaternion() : new Quaternion(90.0f, 90.0f, 0.0f, 0.0f);
    }
}