using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInstruction : MonoBehaviour
{
    public GameObject background;
    private string selectGame;

    // Start is called before the first frame update
    void Start()
    {
        selectGame = PlayerInformation.selectedGame;

        if (selectGame == "1")
        {
            background.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + "4");
        }
        if (selectGame == "2")
        {
            background.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + "5");
        }
        if (selectGame == "3")
        {
            background.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + "6");
        }
    }

    public void Back()
    {
        if(selectGame == "1")
        {
            SceneManager.LoadScene("QuizStartScene");
        }
        else if(selectGame == "2")
        {
            SceneManager.LoadScene("DefenseStartScene");
        }
        else if(selectGame == "3")
        {
            SceneManager.LoadScene("RhythmStartScene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
