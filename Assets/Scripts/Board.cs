
using System.Text.RegularExpressions;
using UnityEngine;

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


    void Awake()
    {
        matchFind = FindAnyObjectByType<MatchFinder>();
    }

    void Start()
    {
        allGems = new GameObject[width,height];

        GridSetUp();


    }

    private void Update()
    {
        matchFind.FindAllMatches();
    }

    private void GridSetUp()
    {
        for(int i = 0; i < width; i++) 
        {
            for(int j = 0; j < height; j++) 
            {
                Vector2 position = new Vector2(i,j);
                GameObject backGroundTile = Instantiate(backGroundTilePrefab,position,Quaternion.identity);
                backGroundTile.transform.parent = transform;
                backGroundTile.name = "("+i.ToString()+","+j.ToString()+")";

                int gemIndex = Random.Range(0,gems.Length);

                int itreations = 0;
                while(MatchesAt(new Vector2Int(i,j),gems[gemIndex]) && itreations < 100)
                {
                    gemIndex = Random.Range(0,gems.Length);
                    itreations++;
                    
                    if(itreations > 0)
                    {
                        Debug.Log(itreations);
                    }
                    
                }

                spawnGems(new Vector2Int(i,j),gems[gemIndex]);

            }
        }
        _cam.transform.position=new Vector3((float)width/2-0.5f,(float)height/2-.5f,-10);
    }

    private void spawnGems(Vector2Int position, GameObject gemSpawn)
    {
        GameObject gem = Instantiate(gemSpawn,new Vector3(position.x , position.y , 0f),Quaternion.identity);
        gem.transform.parent = this.transform;
        gem.name = "Gem" + "("+position.x.ToString()+","+position.y.ToString()+")";

        allGems[position.x,position.y] = gem;
        gem.GetComponent<Gem>().SetUpGem(position,this);
        Gem gemScript = gem.GetComponent<Gem>();
        if (gemScript != null)
        {
            gemScript.SetUpGem(position, this);
        }
        else
        {
            Debug.LogError("Gem script not found on prefab: " + gem.name);
        }

    }

    bool MatchesAt(Vector2Int posToCheck,GameObject gemToCheck)
    {
        if(posToCheck.x > 1)
        {
           if (allGems[posToCheck.x - 1, posToCheck.y].GetComponent<Gem>().type == gemToCheck.GetComponent<Gem>().type && allGems[posToCheck.x - 2, posToCheck.y].GetComponent<Gem>().type == gemToCheck.GetComponent<Gem>().type)
            {
                return true;
            }
        }

        if(posToCheck.y > 1)
        {
           if (allGems[posToCheck.x, posToCheck.y -1].GetComponent<Gem>().type == gemToCheck.GetComponent<Gem>().type && allGems[posToCheck.x, posToCheck.y - 2].GetComponent<Gem>().type == gemToCheck.GetComponent<Gem>().type)
            {
                return true;
            }
        }

        return false;
    }

    private void DestroyMatchedGems(Vector2Int pos)
    {
        if(allGems[pos.x,pos.y] != null)
        {
            if(allGems[pos.x,pos.y].GetComponent<Gem>().isMatched)
            {
                Destroy(allGems[pos.x,pos.y].gameObject);
                allGems[pos.x,pos.y] = null;
            }
        }
    }

    public void DestroyMatches()
    {
        for(int i = 0; i < matchFind.currentMatches.Count;++i)
        {
            if(matchFind.currentMatches[i] != null)
            {
                DestroyMatchedGems(matchFind.currentMatches[i].GetComponent<Gem>().positionIndex);
            }
        }
    }


}
