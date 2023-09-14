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

    private List<TMP_Text> generatedTextFields = new(); // Tablica do przechowywania utworzonych pól tekstowych
    private int selectedMapIndex = 0;
    private int selectedDifficultyIndex = 0;
    private float _menuDelay = 0;

    [SerializeField] private TMP_Text textEasy;
    [SerializeField] private TMP_Text textMedium;
    [SerializeField] private TMP_Text textHard;


    // Start is called before the first frame update
    void Start()
    {
        beatMapListLoader.LoadBeatMaps();

        var y = 265;
        var textWidth = 620; // Nowa szerokość pola tekstowego

        if (textPrefab != null && canvas != null)
        {
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
                newText.rectTransform.position = new Vector3(-420, y, 0);

                newText.rectTransform.SetParent(canvas.transform, false);

                // Ustaw szerokość pola tekstowego na textWidth pikseli
                newText.rectTransform.sizeDelta = new Vector2(textWidth, newText.rectTransform.sizeDelta.y);

                // Ustaw tekst, czcionkę i inne właściwości tekstu według potrzeb.
                newText.text = beatMapDetails.title + " - " + beatMapDetails.author;
                //newText.fontSize = 35;

                //wyłącz pole
                //newText.enabled = false;

                // Dodaj utworzone pole tekstowe do listy
                generatedTextFields.Add(newText);

                // Zmiana pozycji kolejnego tekstu
                y -= 45;
                Debug.Log("Utworzono obiekt: " + newText.name + " na pozycji (" +
                          newText.rectTransform.anchoredPosition.x + ", " + newText.rectTransform.anchoredPosition.y +
                          ")");
            }
        }

        SelectMap(0);
    }

    private void SelectMap(int direction)
    {
        if (direction == -1)
        {
            generatedTextFields[selectedMapIndex - 1].fontSize = 36;
        }
        else if (direction == 1)
        {
            generatedTextFields[selectedMapIndex + 1].fontSize = 36;
        }

        generatedTextFields[selectedMapIndex].fontSize = 42;

        _selectedBeatMap = beatMapListLoader.beatMaps[selectedMapIndex];
        string text = $"Author: {_selectedBeatMap.author}\n" +
                      $"Title: {_selectedBeatMap.title}\n" +
                      $"BeatMap Author: {_selectedBeatMap.beatMapAuthor}\n" +
                      $"Difficulties:";

        
        selectedBeatMapLabel.text = text;

        SelectDifficulty(-1);
        selectedDifficultyIndex = 0;

        //_selectedBeatMap.author + " - " + _selectedBeatMap.title + "(" +
        //                      _selectedBeatMap.beatMapAuthor + ")";
    }

    private void SelectDifficulty(int direction)
    {
        

        if (_selectedBeatMap.difficulties.Count > 0)
        {
            textEasy.text = "Easy";
        }

        if (_selectedBeatMap.difficulties.Count > 1)
        {
            textMedium.text = "Medium";
        }

        if (_selectedBeatMap.difficulties.Count > 2)
        {
            textHard.text = "Hard";
        }

        switch (selectedDifficultyIndex)
        {
            case 0:
                textEasy.color = Color.green;
                break;
            case 1:
                textMedium.color = Color.green;
                break;
            case 2:
                textHard.color = Color.green;
                break;
                
        }
        
        switch (direction)
        {
            case 0:
                textEasy.color = Color.white;
                break;
            case 1:
                textMedium.color = Color.white;
                break;
            case 2:
                textHard.color = Color.white;
                break;
                
        }
        
        _selectedBeatMapDifficulty = _selectedBeatMap.difficulties[selectedDifficultyIndex];
    }

    private void ChangeMap(bool direction) // true - UP, false - DOWN
    {
        if (direction)
        {
            if (selectedMapIndex != 0)
            {
                selectedMapIndex--;
                SelectMap(-1);
            }
        }
        else
        {
            if (selectedMapIndex != generatedTextFields.Count - 1)
            {
                selectedMapIndex++;
                SelectMap(1);
            }
        }
    }

    private void ChangeDifficulty(bool direction) // true - Left, false - Right
    {
        if (direction)
        {
            if (selectedDifficultyIndex != 0)
            {
                selectedDifficultyIndex--;
                SelectDifficulty(selectedDifficultyIndex+1);

            }
        }
        else
        {
            if (selectedDifficultyIndex != _selectedBeatMap.difficulties.Count - 1)
            {
                selectedDifficultyIndex++;
                SelectDifficulty(selectedDifficultyIndex-1);
            }
        }

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