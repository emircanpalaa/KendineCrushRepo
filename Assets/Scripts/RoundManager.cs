using System;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public float roundTime;
    private UI_Manager uiMan;

    private bool endingRound = false;

    private Board board;

    public int currentScore;

    public float displayScore;
    public float scoreSpeed = 5;
    void Awake()
    {
        uiMan = FindAnyObjectByType<UI_Manager>();
        board = FindAnyObjectByType<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        if(roundTime > 0)
        {
            roundTime -= Time.deltaTime;

            if(roundTime <= 0)
            {
                roundTime = 0;

                endingRound = true;

                board.currentState = Board.BoardState.wait;

                
            }
        }

        if(endingRound && board.currentState == Board.BoardState.wait)
        {
            endingRound = false;
            WinCheck();
            
        }

        

        uiMan.timeText.text = roundTime.ToString("0.0") + "";


        displayScore = Mathf.Lerp(displayScore,currentScore,scoreSpeed * Time.deltaTime);
        uiMan.scoreText.text = Mathf.Round(displayScore).ToString();
    }

    private void WinCheck()
    {
        uiMan.roundOverScreen.SetActive(true);
    }

    public float GetRoundTime()
    {
        return roundTime;
    }
}
