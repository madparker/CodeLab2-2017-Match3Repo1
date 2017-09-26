using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManagerScript_dmf463 : MatchManagerScript {

    public override bool GridHasMatch()
    {
        bool match = false;
        //cycle through columns(x position)
        for (int x = 0; x < gameManager.gridWidth; x++)
        {
            //cycle through vertical elements in row
            for (int y = 0; y < gameManager.gridWidth; y++)
            {
                //don't need to evaluate last two rows, because 
                //GridHasHorizontalMatch method is checking last two
                if(x < gameManager.gridWidth - 2)
                {
                    match = match || GridHasHorizontalMatch(x, y);
                }
                if (y < gameManager.gridHeight - 2)
                {
                    //once we've found a match, match will always return true
                    match = match || GridHasVerticalMatch(x, y);
                }
            }
        }
        //Debug.Log("match = "  + match);
        return match;
    }

    public override int RemoveMatches()
    {
        int numRemoved = 0;
        List<GameObject> tokensToDestroy = new List<GameObject>();
        List<Vector2> tokenPos = new List<Vector2>();
        //cycle through row first... 
        for (int x = 0; x < gameManager.gridWidth; x++)
        {
            //then column...
            for (int y = 0; y < gameManager.gridHeight; y++)
            {

                //stop at row(gridHeight - 2) because this is minimum column
                //to check for 3-sprite match
                if (x < gameManager.gridWidth - 2)
                {

                    //Call GetHorizontalMatchLength to get the length of the match
                    int horizonMatchLength = GetHorizontalMatchLength(x, y);

                    //Match must be 3 or more...
                    if (horizonMatchLength > 2)
                    {

                        //...to go through and delete each sprite in this match
                        for (int i = x; i < x + horizonMatchLength; i++)
                        {
                            GameObject token = gameManager.gridArray[i, y];
                            tokensToDestroy.Add(token);
                            tokenPos.Add(new Vector2 (i, y));
                            //gameManager.gridArray[i, y] = null;
                            //record number of items removed
                            numRemoved++;
                        }
                    }
                }
                if (y < gameManager.gridHeight - 2)
                {

                    //Call GetHorizontalMatchLength to get the length of the match
                    int verticalMatchLength = GetVerticalMatchLength(x, y);

                    //Match must be 3 or more...
                    if (verticalMatchLength > 2)
                    {

                        //...to go through and delete each sprite in this match
                        for (int i = y; i < y + verticalMatchLength; i++)
                        {
                            GameObject token = gameManager.gridArray[x, i];
                            tokensToDestroy.Add(token);
                            tokenPos.Add(new Vector2(x, i));
                            //gameManager.gridArray[x, i] = null;
                            //record number of items removed
                            numRemoved++;
                        }
                    }
                }
            }
        }
        foreach (GameObject token in tokensToDestroy)
        {
            Destroy(token);
        }
        for (int i = 0; i < tokenPos.Count; i++)
        {
            gameManager.gridArray[(int)tokenPos[i].x, (int)tokenPos[i].y] = null;
        }
        return numRemoved;
    }

    //check
    public bool GridHasVerticalMatch(int x, int y)
    {
        //reference to 3 game objects along y row, starting from x
        GameObject token1 = gameManager.gridArray[x, y + 0];
        GameObject token2 = gameManager.gridArray[x, y + 1];
        GameObject token3 = gameManager.gridArray[x, y + 2];

        if (token1 != null && token2 != null && token3 != null)
        {
            //after null check, evaluate match among the three sprites
            SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
            SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
            SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();

            return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
        }
        else
        {
            return false;
        }
    }

    public int GetVerticalMatchLength(int x, int y)
    {
        int matchLength = 1;

        GameObject first = gameManager.gridArray[x, y];

        if (first != null)
        {
            //null check, then get first sprite
            SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();

            //evaluate along subsequent rows in this column to get match length
            for (int i = y + 1; i < gameManager.gridHeight; i++)
            {
                GameObject other = gameManager.gridArray[x, i];
                if (other != null)
                {
                    SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();
                    if (sr1.sprite == sr2.sprite)
                    {
                        matchLength++;
                    }
                    else
                    {
                        //break out of loop if sprites inequal...
                        break;
                    }
                }
                else
                {
                    //break immediately if null...
                    break;
                }
            }
        }
        //... then return the length of the match
        return matchLength;
    }

}
