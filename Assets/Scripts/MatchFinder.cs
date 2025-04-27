using System.Collections.Generic;
using System.IO.Compression;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

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
        //currentMatches.Clear();

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
    }
}
