using System;
using System.Collections;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class Gem : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int positionIndex;
    [HideInInspector]
    public Board board;


    private Vector2 firstTouchPos;
    private Vector2 finalTouchPos;
    private bool mouseClicked;
    private float swipeAngle;

    private GameObject otherGem;


    public enum GemType {Blue,Green,Red,Yellow,Purple};
    public GemType type;
    public bool isMatched = false;
    [HideInInspector]
    public Vector2Int previousPosition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position,positionIndex) > .01f)
        {
            transform.position = Vector2.Lerp((Vector2)transform.position,positionIndex,board.gemSpeed * Time.deltaTime);
        }
        else
        {
            transform.position =new Vector3 (positionIndex.x,positionIndex.y,0f);
            board.allGems[positionIndex.x,positionIndex.y] = this.gameObject;
        }
        

        if(mouseClicked && Input.GetMouseButtonUp(0))
        {
            mouseClicked = false;
            finalTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            AngleCalculator();
        }
        
    }

    public void SetUpGem(Vector2Int position,Board _board)
    {
        positionIndex = position;
        board = _board;

    }

    private void OnMouseDown()
    {
        firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseClicked = true;
    }

    private void AngleCalculator()
    {
        swipeAngle = Mathf.Atan2(finalTouchPos.y - firstTouchPos.y , finalTouchPos.x - firstTouchPos.x);
        swipeAngle = swipeAngle * 180 /Mathf.PI;
        Debug.Log(swipeAngle);
        
        if(Vector3.Distance(firstTouchPos,finalTouchPos) > .5f)
        {
            MoveGems();
        }
    }

    private void MoveGems()
    {
        previousPosition = positionIndex;

        if(swipeAngle < 45 && swipeAngle > -45 && positionIndex.x < board.width - 1)
        {
            otherGem = board.allGems[positionIndex.x + 1,positionIndex.y];
            otherGem.GetComponent<Gem>().positionIndex.x--;
            positionIndex.x++;
        }
        else if(swipeAngle > 45 && swipeAngle <= 135 && positionIndex.y < board.height - 1)
        {
            otherGem = board.allGems[positionIndex.x,positionIndex.y + 1];
            otherGem.GetComponent<Gem>().positionIndex.y--;
            positionIndex.y++;
        }
        else if(swipeAngle < -45 && swipeAngle >= -135 && positionIndex.y > 0)
        {
            otherGem = board.allGems[positionIndex.x,positionIndex.y - 1];
            otherGem.GetComponent<Gem>().positionIndex.y++;
            positionIndex.y--;
        }
        else if(swipeAngle > 135 || swipeAngle < -135 && positionIndex.x > 0)
        {
            otherGem = board.allGems[positionIndex.x - 1,positionIndex.y];
            otherGem.GetComponent<Gem>().positionIndex.x++;
            positionIndex.x--;
        }

        board.allGems[positionIndex.x,positionIndex.y] = this.gameObject;
        board.allGems[otherGem.GetComponent<Gem>().positionIndex.x,otherGem.GetComponent<Gem>().positionIndex.y] = otherGem;

        StartCoroutine(CheckMove());
    }

    public IEnumerator CheckMove()
    {
        yield return new WaitForSeconds(.5f);

        board.matchFind.FindAllMatches();

        if(otherGem != null )
        {
            if(!isMatched && !otherGem.GetComponent<Gem>().isMatched)
            {
                otherGem.GetComponent<Gem>().positionIndex = positionIndex;
                positionIndex = previousPosition;
            }
            else
            {
                board.DestroyMatches();
            }
        }
    }
}
