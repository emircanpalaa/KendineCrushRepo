
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public float roundTime;
    private UI_Manager uiMan;

    private bool endingRound = false;

    private Board board;

    public int currentScore;

    public float displayScore;
    public float scoreSpeed = 5;

    public int scoreTarget_1, scoreTarget_2, scoreTarget_3;
    void Awake()
    {
        uiMan = FindAnyObjectByType<UI_Manager>();
        board = FindAnyObjectByType<Board>();
    }

  
    void Update()
    {
        if (roundTime > 0)
        {
            roundTime -= Time.deltaTime;

            if (roundTime <= 0)
            {
                roundTime = 0;

                endingRound = true;

                board.currentState = Board.BoardState.wait;


            }
        }

        if (endingRound && board.currentState == Board.BoardState.wait)
        {
            endingRound = false;
            WinCheck();

        }



        uiMan.timeText.text = roundTime.ToString("0.0") + "";


        displayScore = Mathf.Lerp(displayScore, currentScore, scoreSpeed * Time.deltaTime);
        uiMan.scoreText.text = Mathf.Round(displayScore).ToString();
    }

    private void WinCheck()
    {
        uiMan.roundOverScreen.SetActive(true);

        uiMan.winScore.text = currentScore.ToString();

        int levelIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentScore >= scoreTarget_3)
        {
            uiMan.winStar_3.SetActive(true);

        }

        else if (currentScore >= scoreTarget_2)
        {
            uiMan.winStar_2.SetActive(true);

        }

        else if (currentScore >= scoreTarget_1)
        {
            uiMan.winStar_1.SetActive(true);
        }   

        SFXManager.Instance.PlayRoundOver();

        DestroyAllGemsBeforeSceneLoad();
        

    }

    public float GetRoundTime()
    {
        return roundTime;
    }

    private void DestroyAllGemsBeforeSceneLoad()
{
    // Tüm Gem objelerini sahnede bul ve yok et
    Gem[] allGemsInScene = FindObjectsByType<Gem>(FindObjectsSortMode.None);
    foreach (Gem gem in allGemsInScene)
    {
        Destroy(gem.gameObject);
    }
}
}
