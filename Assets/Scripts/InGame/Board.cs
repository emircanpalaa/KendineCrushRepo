
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{

    public int width;
    public int height;

    public GameObject backGroundTilePrefab;
    public GameObject[] gems;
    public GameObject[,] allGems;
    public float gemSpeed;
    public Transform _cam;

    public MatchFinder matchFind;

    public enum BoardState { wait, move };
    public BoardState currentState = BoardState.move;

    public GameObject bomb;
    public float bombChance = 2f;

    [HideInInspector]
    public RoundManager roundMan;

    private float _bonusMulti;
    public float bonusAmount = .5f;

    private BoardLayout boardLayout;
    private GameObject[,] layoutStore;


    void Awake()
    {
        // matchFind = FindAnyObjectByType<MatchFinder>();
        // roundMan = FindAnyObjectByType<RoundManager>();

        // boardLayout = GetComponent<BoardLayout>();
    }

    void Start()
    {
        allGems = new GameObject[width, height];

        layoutStore = new GameObject[width, height];
        GridSetUp();


    }

    private void Update()
    {
        //matchFind.FindAllMatches();

        if (Input.GetKeyDown(KeyCode.S))
        {
            ShuffleTheBoard();
        }
    }


    private void GridSetUp()
    {
        if (boardLayout != null)
        {
            layoutStore = boardLayout.GetLayout();
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 position = new Vector2(i, j);
                GameObject backGroundTile = Instantiate(backGroundTilePrefab, position, Quaternion.identity);
                backGroundTile.transform.parent = transform;
                backGroundTile.name = "(" + i.ToString() + "," + j.ToString() + ")";

                if (layoutStore[i, j] != null)
                {
                    spawnGems(new Vector2Int(i, j), layoutStore[i, j]);
                }
                else
                {
                    int gemIndex = Random.Range(0, gems.Length);

                    int itreations = 0;
                    while (MatchesAt(new Vector2Int(i, j), gems[gemIndex]) && itreations < 100)
                    {
                        gemIndex = Random.Range(0, gems.Length);
                        itreations++;

                        if (itreations > 0)
                        {
                            Debug.Log(itreations);
                        }

                    }

                    spawnGems(new Vector2Int(i, j), gems[gemIndex]);
                }



            }
        }
        _cam.transform.position = new Vector3((float)width * -.035f, (float)height / 2 - .5f, -5f);
    }

    private void spawnGems(Vector2Int position, GameObject gemSpawn)
    {
        if (Random.Range(0f, 100f) < bombChance)
        {
            gemSpawn = bomb;
        }

        GameObject gem = Instantiate(gemSpawn, new Vector3(position.x, position.y + height, 0f), Quaternion.identity);
        gem.transform.parent = this.transform;
        gem.name = "Gem" + "(" + position.x.ToString() + "," + position.y.ToString() + ")";

        allGems[position.x, position.y] = gem;
        gem.GetComponent<Gem>().SetUpGem(position, this);
        Gem gemScript = gem.GetComponent<Gem>();
        
    }

    bool MatchesAt(Vector2Int posToCheck, GameObject gemToCheck)
    {
        if (posToCheck.x > 1)
        {
            if (allGems[posToCheck.x - 1, posToCheck.y].GetComponent<Gem>().type == gemToCheck.GetComponent<Gem>().type && allGems[posToCheck.x - 2, posToCheck.y].GetComponent<Gem>().type == gemToCheck.GetComponent<Gem>().type)
            {
                return true;
            }
        }

        if (posToCheck.y > 1)
        {
            if (allGems[posToCheck.x, posToCheck.y - 1].GetComponent<Gem>().type == gemToCheck.GetComponent<Gem>().type && allGems[posToCheck.x, posToCheck.y - 2].GetComponent<Gem>().type == gemToCheck.GetComponent<Gem>().type)
            {
                return true;
            }
        }

        return false;
    }

    private void DestroyMatchedGems(Vector2Int pos)
    {
        if (allGems[pos.x, pos.y] != null)
        {
            Gem gem = allGems[pos.x, pos.y].GetComponent<Gem>();
            if (gem != null && gem.isMatched)
            {
                if(allGems[pos.x,pos.y].GetComponent<Gem>().type == Gem.GemType.Bomb)
                {
                    SFXManager.Instance.PlayExplode();
                }
                else if(allGems[pos.x,pos.y].GetComponent<Gem>().type == Gem.GemType.Stone)
                {
                    SFXManager.Instance.PlayStone();
                }
                else
                {
                    SFXManager.Instance.PlayGemBreak();
                }

                if (gem.destroyEffect != null)
                {
                    Instantiate(gem.destroyEffect, new Vector2(pos.x, pos.y), Quaternion.identity);
                }

                Destroy(allGems[pos.x, pos.y].gameObject);
                allGems[pos.x, pos.y] = null;
            }
        }
    }


    public void DestroyMatches()
    {
        for (int i = 0; i < matchFind.currentMatches.Count; ++i)
        {
            if (matchFind.currentMatches[i] != null)
            {
                ScoreCheck(matchFind.currentMatches[i]);
                //ScoreMultipler(matchFind.currentMatches[i]);

                DestroyMatchedGems(matchFind.currentMatches[i].GetComponent<Gem>().positionIndex);
            }
        }
        StartCoroutine(DecreaseRow());
    }

    private IEnumerator DecreaseRow()
    {
        yield return new WaitForSeconds(.2f);

        int nullCounter = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allGems[x, y] == null)
                {
                    nullCounter++;
                }
                else if (nullCounter > 0)
                {
                    allGems[x, y].GetComponent<Gem>().positionIndex.y -= nullCounter;
                    allGems[x, y - nullCounter] = allGems[x, y];
                    allGems[x, y] = null;
                }
            }
            nullCounter = 0;
        }
        StartCoroutine(FillBoardCoroutine());
    }

    private IEnumerator FillBoardCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        RefillBoard();

        yield return new WaitForSeconds(.5f);
        matchFind.FindAllMatches();

        if (matchFind.currentMatches.Count > 0)
        {
            _bonusMulti++;

            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        else
        {
            currentState = BoardState.move;

            _bonusMulti = 0f;
        }
    }


    private void RefillBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allGems[x, y] == null)
                {
                    int gemIndex = Random.Range(0, gems.Length);

                    spawnGems(new Vector2Int(x, y), gems[gemIndex]);
                }

            }
        }
        CheckMisPlacedGems();
    }

    private void CheckMisPlacedGems()
    {
        // Sadece sahnedeki Gem componentli objeleri bul
        Gem[] foundGems = FindObjectsByType<Gem>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        List<GameObject> misplacedGems = new List<GameObject>();

        foreach (Gem gem in foundGems)
        {
            if (gem != null && !IsGemInAllGems(gem.gameObject))
            {
                misplacedGems.Add(gem.gameObject);
            }
        }

        foreach (GameObject g in misplacedGems)
        {
            Destroy(g.gameObject);
        }
    }

    private bool IsGemInAllGems(GameObject gem)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allGems[x, y] == gem)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void ShuffleTheBoard()
    {
        if (currentState != BoardState.wait)
        {
            currentState = BoardState.wait;

            List<GameObject> gemsFromBoard = new List<GameObject>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    gemsFromBoard.Add(allGems[x, y]);
                    allGems[x, y] = null;
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int gemToUse = Random.Range(0, gemsFromBoard.Count);

                    int itreations = 0;
                    while (MatchesAt(new Vector2Int(x, y), gemsFromBoard[gemToUse]) && itreations < 100 && gemsFromBoard.Count > 1)
                    {
                        gemToUse = Random.Range(0, gemsFromBoard.Count);
                        itreations++;
                    }

                    gemsFromBoard[gemToUse].GetComponent<Gem>().SetUpGem(new Vector2Int(x, y), this);
                    allGems[x, y] = gemsFromBoard[gemToUse];
                    gemsFromBoard.RemoveAt(gemToUse);
                }
            }

            StartCoroutine(FillBoardCoroutine());

            currentState = Board.BoardState.move;

        }
    }

    public void ShuffleButtonPressed()
    {
        do
        {
            ShuffleTheBoard();
            matchFind.FindAllMatches();
        }
        while (matchFind.currentMatches.Count > 0);

    }

    public void ScoreCheck(GameObject gemToCheck)
    {
        roundMan.currentScore += gemToCheck.GetComponent<Gem>().gemValue;

        if (_bonusMulti > 0)
        {
            float bonusToAdd = gemToCheck.GetComponent<Gem>().gemValue * _bonusMulti * bonusAmount;
            roundMan.currentScore += Mathf.RoundToInt(bonusToAdd);
        }

    }

    // Board.cs içinden taş oluşturuluyor
void SpawnNewGemAt(int x, int y)
{
    int gemToUse = Random.Range(0, allGems.Length);
    GameObject gem = Instantiate(gems[gemToUse], new Vector2(x, y + 5), Quaternion.identity); // Yukarıdan başlasın
    Gem gemScript = gem.GetComponent<Gem>();
    gemScript.board = this;
    gemScript.positionIndex = new Vector2Int(x, y); // Hedef pozisyon
    allGems[x, y] = gem;
}













}
