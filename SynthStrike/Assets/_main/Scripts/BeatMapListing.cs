using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BeatMapListing : MonoBehaviour
{
    [SerializeField] private BeatMapListLoader beatMapListLoader;
    [SerializeField] private TMP_Text selectedBeatMapLabel;
    private BeatMapDetails _selectedBeatMap;
    private string _selectedBeatMapDifficulty = string.Empty;

    // Start is called before the first frame update
    void Start()
    {
        beatMapListLoader.LoadBeatMaps();
        _selectedBeatMap = beatMapListLoader.beatMaps[0];
        selectedBeatMapLabel.text = _selectedBeatMap.author + " - " + _selectedBeatMap.title + "(" +
                                     _selectedBeatMap.beatMapAuthor + ")";
        _selectedBeatMapDifficulty = _selectedBeatMap.difficulties[0];
    }

    // Update is called once per frame
    void Update()
    {
    }
}