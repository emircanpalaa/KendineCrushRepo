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

    void Start()
    {
        winStar_1.SetActive(false);
        winStar_2.SetActive(false);
        winStar_3.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
