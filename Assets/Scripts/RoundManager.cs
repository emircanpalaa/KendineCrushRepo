using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public float roundTime = 60f;
    private UI_Manager uiMan;

    private bool endingRound = false;

    private Board board;

    public int currentScore;
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

                
            }
        }

        if(endingRound && board.currentState == Board.BoardState.wait)
        {
            endingRound = false;
            WinCheck();
            
        }

        

        uiMan.timeText.text = roundTime.ToString("0.0") + "";

        uiMan.scoreText.text = currentScore.ToString();
    }

    private void WinCheck()
    {
        uiMan.roundOverScreen.SetActive(true);
    }
}
