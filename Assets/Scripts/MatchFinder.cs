using System.Collections.Generic;
using System.IO.Compression;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using JetBrains.Annotations;

public class MatchFinder : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();

    void Awake()
    {
        board = FindAnyObjectByType<Board>();
    }

    public void FindAllMatches()
    {
        currentMatches.Clear();

        for(int i = 0; i < board.width; i++) 
        {
            for(int j = 0; j < board.height; j++) 
            {
                GameObject currentGem = board.allGems[i,j];
                if(currentGem != null)
                {
                    if(i > 0 && i < board.width - 1)
                    {
                        GameObject leftGem = board.allGems[i-1,j];
                        GameObject rightGem = board.allGems[i + 1,j];

                        if(leftGem != null && rightGem != null)
                        {
                            if(leftGem.GetComponent<Gem>().type == currentGem.GetComponent<Gem>().type &&
                                rightGem.GetComponent<Gem>().type == currentGem.GetComponent<Gem>().type)
                                {
                                    currentGem.GetComponent<Gem>().isMatched = true;
                                    leftGem.GetComponent<Gem>().isMatched = true;
                                    rightGem.GetComponent<Gem>().isMatched = true;

                                    currentMatches.Add(currentGem);
                                    currentMatches.Add(leftGem);
                                    currentMatches.Add(rightGem);
                                }

                                if (currentGem.GetComponent<Gem>().type == Gem.GemType.Bomb)
                                {
                                    MarkBombArea(currentGem.GetComponent<Gem>().positionIndex, currentGem);
                                }



                        }
                    }
                    if(j > 0 && j < board.height - 1)
                    {
                        GameObject aboveGem = board.allGems[i,j + 1];
                        GameObject belowGem = board.allGems[i,j - 1];

                        if(aboveGem != null && belowGem != null)
                        {
                            if(aboveGem.GetComponent<Gem>().type == currentGem.GetComponent<Gem>().type &&
                                belowGem.GetComponent<Gem>().type == currentGem.GetComponent<Gem>().type)
                                {
                                    currentGem.GetComponent<Gem>().isMatched = true;
                                    aboveGem.GetComponent<Gem>().isMatched = true;
                                    belowGem.GetComponent<Gem>().isMatched = true;

                                    currentMatches.Add(currentGem);
                                    currentMatches.Add(aboveGem);
                                    currentMatches.Add(belowGem);
                                }

                        }
                    }
                }
            }
        }

        if(currentMatches.Count > 0 )
        {
            currentMatches = currentMatches.Distinct().ToList();
        }

        CheckForBombs();
    }

    public void CheckForBombs()
    {
        for(int i = 0;i < currentMatches.Count;i++)
        {
            GameObject gem = currentMatches[i];

            int x = gem.GetComponent<Gem>().positionIndex.x;
            int y = gem.GetComponent<Gem>().positionIndex.y;

            if(gem.GetComponent<Gem>().positionIndex.x > 0)
            {
                if(board.allGems[x-1,y] != null)
                {
                    if (board.allGems[x-1, y].GetComponent<Gem>().type == Gem.GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x-1,y),board.allGems[x-1,y]);
                    }
                }
            }

            if(gem.GetComponent<Gem>().positionIndex.x < board.width - 1)
            {
                if(board.allGems[x + 1,y] != null)
                {
                    if (board.allGems[x + 1, y].GetComponent<Gem>().type == Gem.GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x + 1,y),board.allGems[x + 1,y]);
                    }

                }
            }

            if(gem.GetComponent<Gem>().positionIndex.y > 0)
            {
                if(board.allGems[x,y - 1] != null)
                {
                    if (board.allGems[x, y - 1].GetComponent<Gem>().type == Gem.GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x,y - 1),board.allGems[x,y - 1]);
                    }
                }
            }

            if(gem.GetComponent<Gem>().positionIndex.y < board.height - 1)
            {
                if(board.allGems[x,y + 1] != null)
                {
                    if (board.allGems[x, y + 1].GetComponent<Gem>().type == Gem.GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x,y + 1),board.allGems[x,y + 1]);
                    }

                }
            }
        }
    }

    public void MarkBombArea(Vector2Int bombPosition,GameObject theBomb)
    {
        for(int x = bombPosition.x - theBomb.GetComponent<Gem>()._blastSize; x <= bombPosition.x + theBomb.GetComponent<Gem>()._blastSize;x++)
        {
            for(int y = bombPosition.y - theBomb.GetComponent<Gem>()._blastSize; y <= bombPosition.y + theBomb.GetComponent<Gem>()._blastSize;y++)
            {
                if(x >= 0 && x < board.width && y >= 0 && y < board.height)
                {
                    if(board.allGems[x,y] != null)
                    {
                        board.allGems[x,y].GetComponent<Gem>().isMatched = true;
                        currentMatches.Add(board.allGems[x,y]);
                    }
                }
            }
        }

        currentMatches = currentMatches.Distinct().ToList();
    }
}
