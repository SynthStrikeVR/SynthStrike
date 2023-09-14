using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR;

public class BeatMapListing : MonoBehaviour
{
    [SerializeField] private BeatMapListLoader beatMapListLoader;
    [SerializeField] private TMP_Text selectedBeatMapLabel;
    private BeatMapDetails _selectedBeatMap;
    private string _selectedBeatMapDifficulty = string.Empty;

    [SerializeField] private TMP_Text textPrefab; // Przypisz prefab TextMeshPro do tego pola w inspektorze.
    [SerializeField] private Canvas canvas;
    //[SerializeField] private Scrollbar scrollbar;

    private List<TMP_Text> generatedTextFields = new(); // Tablica do przechowywania utworzonych pól tekstowych
    private int selectedMapIndex = 0;
    private int selectedDifficultyIndex = 0;
    
    
    private float _menuDelay = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        beatMapListLoader.LoadBeatMaps();
        // _selectedBeatMap = beatMapListLoader.beatMaps[selectedMapIndex];
        // selectedBeatMapLabel.text = _selectedBeatMap.author + " - " + _selectedBeatMap.title + "(" +
        //                               _selectedBeatMap.beatMapAuthor + ")";
        SelectMap();
        SelectDifficulty();
        //_selectedBeatMapDifficulty = _selectedBeatMap.difficulties[selectedDifficultyIndex];

        var y = 265;
        var textWidth = 1000; // Nowa szerokość pola tekstowego

        if (textPrefab != null && canvas != null)
        {
            // Pobierz komponent ScrollRect
            ScrollRect scrollRect = canvas.GetComponentInChildren<ScrollRect>();

            if (scrollRect != null)
            {
                // Pobierz RectTransform obszaru treści
                RectTransform contentRect = scrollRect.content;

                // Usuń wszystkie dzieci z obszaru treści
                foreach (Transform child in contentRect)
                {
                    Destroy(child.gameObject);
                }
                
                foreach (var beatMapDetails in beatMapListLoader.beatMaps)
                {
                    // Tworzenie nowego obiektu TMP_Text przy użyciu Instantiate.
                    TMP_Text newText = Instantiate(textPrefab, Vector3.zero, Quaternion.identity);

                    newText.name = "Text - " + beatMapDetails.title;
                    foreach (Transform child in newText.transform)
                    {
                         Destroy(child.gameObject);
                    }
                    // Ustaw pozycję tekstu w obrębie obszaru treści
                    newText.rectTransform.SetParent(contentRect, false);
                    newText.rectTransform.anchoredPosition = new Vector2(-380, y);

                    // Ustaw szerokość pola tekstowego na textWidth pikseli
                    newText.rectTransform.sizeDelta = new Vector2(textWidth, newText.rectTransform.sizeDelta.y);

                    // Ustaw tekst, czcionkę i inne właściwości tekstu według potrzeb.
                    newText.text = beatMapDetails.title + " - " + beatMapDetails.author;
                    newText.fontSize = 70;

                    //wyłącz pole
                    //newText.enabled = false;
                    
                    // Dodaj utworzone pole tekstowe do listy
                    generatedTextFields.Add(newText);

                    // Zmiana pozycji kolejnego tekstu
                    y -= 75;
                    Debug.Log("Utworzono obiekt: " + newText.name + " na pozycji (" + newText.rectTransform.anchoredPosition.x + ", " + newText.rectTransform.anchoredPosition.y + ")");
                }
                
            }
        }
        
        
    }

    private void SelectMap()
    {
        // for (int i = selectedMapIndex - 2; i <= selectedMapIndex + 2; i++)
        // {
        //     if (i >= 0 && i < generatedTextFields.Count)
        //     {
        //         generatedTextFields[i].enabled = true;
        //     }
        // }
    

        _selectedBeatMap = beatMapListLoader.beatMaps[selectedMapIndex];
        selectedBeatMapLabel.text = _selectedBeatMap.author + " - " + _selectedBeatMap.title + "(" +
                                    _selectedBeatMap.beatMapAuthor + ")";
    }

    private void SelectDifficulty()
    {
        _selectedBeatMapDifficulty = _selectedBeatMap.difficulties[selectedDifficultyIndex];
    }
    
    private void ChangeMap(bool direction) // true - UP, false - DOWN
    {
        if (direction)
        {
            if (selectedMapIndex != 0)
            {
                selectedMapIndex--;
            }
        }
        else
        {
            if (selectedMapIndex != generatedTextFields.Count - 1)
            {
                selectedMapIndex++;
            }
        }

        SelectMap();

    }

    private void ChangeDifficulty(bool direction) // true - Left, false - Right
    {
        if (direction)
        {
            if (selectedDifficultyIndex != 0)
            {
                selectedDifficultyIndex--;
            }
        }
        else
        {
            if (selectedDifficultyIndex != _selectedBeatMap.difficulties.Count - 1)
            {
                selectedDifficultyIndex++;
            }
        }
        
        SelectDifficulty();
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        _menuDelay += Time.deltaTime;
        if (_menuDelay > 0.3)
        {
            var vertical = Input.GetAxis("XRI_Right_Primary2DAxis_Vertical");
    
            var horizontal = Input.GetAxis("XRI_Right_Primary2DAxis_Horizontal");
            Debug.Log("hor" + horizontal + " ver" + vertical);
            if (vertical >= 0.9)
            {
                ChangeMap(true);
            }
            else
            {
                if (vertical <= -0.9)
                {
                    ChangeMap(false);
                }
                else
                {
                    if (horizontal >= 0.9)
                    {
                        ChangeDifficulty(true);
                    }
                    else
                    {
                        if (horizontal <= -0.9)
                        {
                            ChangeDifficulty(false);
                        }
                    }
                }
            }
            
            _menuDelay = 0;
        }
    }
}

