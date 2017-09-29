using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGreatMatchManagerExtensionByDennis : MatchManagerScript {

    bool firstMatchy;
    float savedLerpSpeed;
    bool lerpSpeedSaved;


    // Check to see if there is a match anywhere on the grid.
    public override bool GridHasMatch()
    {
        bool match = false;
        if (!firstMatchy)
        {
            Camera.main.orthographicSize = 9999999;

            if (!lerpSpeedSaved)
            {
                savedLerpSpeed = FindObjectOfType<MoveTokensScript>().lerpSpeed;
                lerpSpeedSaved = true;
            }

            FindObjectOfType<MoveTokensScript>().lerpSpeed = 9999999999999999999;
        }

        else
        {
            FindObjectOfType<MoveTokensScript>().lerpSpeed = savedLerpSpeed;
        }

        // Cycle through all columns (x position).
        for (int x = 0; x < gameManager.gridWidth; x++)
        {
            // Cycle through all horizontal tokens in the current column.
            for (int y = 0; y < gameManager.gridHeight; y++)
            {
                // Don't need to evaluate last two rows or columns, because they might not even actually exist and that would be bad.
                if (x < gameManager.gridWidth - 2)
                {
                    match = match || GridHasHorizontalMatch(x, y);
                }

                if (y < gameManager.gridHeight - 2)
                {
                    match = match || GridHasVerticalMatch(x, y);
                }
            }
        }

        if (!match)
        {
            firstMatchy = true;
            Camera.main.orthographicSize = 5;
        }

        return match;
    }


    public override int RemoveMatches()
    {
        int numRemoved = 0;

        if (firstMatchy)
        {
            List<Rigidbody2D> rigidbodies = new List<Rigidbody2D>();

            foreach (SpriteRenderer spriteRenderer in FindObjectsOfType<SpriteRenderer>())
            {
                spriteRenderer.gameObject.AddComponent<Rigidbody2D>();
                rigidbodies.Add(spriteRenderer.GetComponent<Rigidbody2D>());
            }

            foreach (Rigidbody2D rigidybody in rigidbodies)
            {
                rigidybody.AddForce(Random.insideUnitSphere * Random.Range(50f, 100f), ForceMode2D.Impulse);
            }
        }

        // Cycle through the grid.
        for (int x = 0; x < gameManager.gridWidth; x++)
        {
            for (int y = 0; y < gameManager.gridHeight; y++)
            {
                // Don't check tokens that don't exist, silly! Ha ha! You're so dumb!
                if (x < gameManager.gridWidth - 2 || y < gameManager.gridHeight - 2)
                {
                    // Call GetHorizontalMatchLength to get the length of the match
                    int horizontalMatchLength = GetyHorizontalyMatchyLengthy(x, y);
                    int verticalMatchLength = GetVerticalMatchLength(x, y);

                    // Make sure the match length was at least 3 because the name of the fucking genre is Match 3 and if we allow the player to match 2 then what kind of world are we even living in?
                    if (horizontalMatchLength > 2)
                    {
                        // Go through and delete each sprite in this match
                        for (int i = x; i < x + horizontalMatchLength; i++)
                        {
                            GameObject token = gameManager.gridArray[i, y];
                            Destroy(token);

                            gameManager.gridArray[i, y] = null;

                            //record number of items removed
                            numRemoved++;
                        }
                    }

                    // Do that same thing but for vertical matches.
                    if (verticalMatchLength > 2)
                    {
                        // Go through and delete each sprite in this match
                        for (int i = y; i < y + verticalMatchLength; i++)
                        {
                            GameObject token = gameManager.gridArray[x, i];
                            Destroy(token);

                            gameManager.gridArray[x, i] = null;

                            //record number of items removed
                            numRemoved++;
                        }
                    }
                }
            }
        }

        return numRemoved;
    }


    public int GetyHorizontalyMatchyLengthy(int x, int y)
    {
        int matchLength = 1;

        GameObject first = gameManager.gridArray[x, y];

        if (first != null)
        {
            //null check, then get first sprite
            SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();

            //evaluate along subsequent columns in this row to get match length
            for (int i = x + 1; i < gameManager.gridWidth; i++)
            {
                GameObject other = gameManager.gridArray[i, y];

                if (other != null)
                {
                    SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

                    if (sr1.sprite.name == sr2.sprite.name/* && !sr1.sprite.name.Contains("Locked") && !sr2.sprite.name.Contains("Locked")*/)
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


    public int GetVerticalMatchLength(int x, int y)
    {
        int matchLength = 1;

        GameObject firstToken = gameManager.gridArray[x, y];

        if (firstToken != null)
        {
            // Null check, then get first sprite
            SpriteRenderer sr1 = firstToken.GetComponent<SpriteRenderer>();

            // Evaluate along subsequent rows in this column to get match length
            for (int i = y + 1; i < gameManager.gridHeight; i++)
            {
                GameObject other = gameManager.gridArray[x, i];

                // Check to see if the sprites match.
                if (other != null)
                {
                    SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

                    // If they do match, increase the return value by one.
                    if (sr1.sprite.name == sr2.sprite.name/* && !sr1.sprite.name.Contains("Locked") && !sr2.sprite.name.Contains("Locked")*/)
                    {
                        matchLength++;
                    }

                    else
                    {
                        break;
                    }
                }

                // If the sprites don't match then break immediately.
                else
                {
                    break;
                }
            }
        }

        // Now that the ransom has been extracted, return Mr. Match Length to his wife and children with only slight physical injury and mild to moderate psychological trauma.
        return matchLength;
    }


    public bool GridHasVerticalMatch(int x, int y)
    {
        // References to 3 game objects along X column, starting from row Y
        GameObject token1 = gameManager.gridArray[x, y + 0];
        GameObject token2 = gameManager.gridArray[x, y + 1];
        GameObject token3 = gameManager.gridArray[x, y + 2];

        if (token1 != null && token2 != null && token3 != null)
        {
            // After a null check, evaluate this match by seeing if the sprites of each token match.
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
}
