using UnityEngine;
using TMPro;


public class UI_Manager : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text scoreText;

    public TMP_Text winScore;
    public TMP_Text winText;
    public GameObject winStar_1,winStar_2,winStar_3;

    public GameObject roundOverScreen;

    public GameObject pauseScreen;

    void Start()
    {
        winStar_1.SetActive(false);
        winStar_2.SetActive(false);
        winStar_3.SetActive(false);

    }


    void Update()
    {
        
    }

    public void PauseUnPause()
    {
        if(!pauseScreen.activeInHierarchy)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
            
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
