using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AGreatMoveTokensScriptExtensionByDennis : MoveTokensScript {

    bool userSwap_;

    GameObject exchangeToken1_;
    GameObject exchangeToken2_;

    Vector2 exchangeGridPos1_;
    Vector2 exchangeGridPos2_;

    [SerializeField] Sprite lockSprite;

    int points = 0;


    // Recieves selected tokens from InputManagerScript and sets proper variables in preparation for the exchange.
    public void SetupyTokenyExchangey(GameObject token1, Vector2 pos1,
                                   GameObject token2, Vector2 pos2, bool reversable)
    {
        SetupTokenMove();

        // Set up references to each token object which will be swapped.
        exchangeToken1 = token1;
        exchangeToken1_ = token1;
        exchangeToken2_ = token2;

        // Sete up references to where each token will end up.
        exchangeGridPos1_ = pos1;
        exchangeGridPos2_ = pos2;

        this.userSwap_ = reversable;
    }


    // Go through all tokens and see if their above an empty space, if so, call MoveTokenToEmptyPos() on it.
    // Returns true if all moves have finished.
    public override bool MoveTokensToFillEmptySpaces()
    {
        bool movedToken = false;

        // Go through entire grid.
        for (int x = 0; x < gameManager.gridWidth; x++)
        {
            for (int y = 1; y < gameManager.gridHeight; y++)
            {
                // See how far down this piece should drop.
                int dropLength = GetDropLength(x, y);

                // See if the token at the current grid space is above an empty grid space.
                if (dropLength > 0)
                {
                    for (int pos = y; pos < gameManager.gridHeight; pos++)
                    {
                        GameObject token = gameManager.gridArray[x, pos];
                        if (token != null)
                        {
                            MoveTokenToEmptyPos(x, pos, x, pos - dropLength, token);
                            movedToken = true;
                        }
                    }
                }
            }
        }

        //If the tokens have reached their final position, stop moving them.
        if (lerpPercent == 1)
        {
            move = false;
        }

        return movedToken;
    }


    int GetDropLength(int x, int y)
    {
        int dropLength = 0;

        // Evaluate along subsequent rows in this column to get drop length
        for (int i = y - 1; i >= 0; i--)
        {
            // Check to see if this grid space is empty.
            if (gameManager.gridArray[x, i] == null)
            {
                dropLength++;
            }

            // If this position is not empty then break immediately.
            else
            {
                break;
            }
        }

        //Debug.Log("Drop Length: " + dropLength);

        // Now that the ransom has been extracted, return Mrs. Drop Length to her wife and children.
        return dropLength;
    }


    // This function is responsible for moving the tokens and resetting the variables if there was a match, and putting them back into their previous positions
    // if there was not a match.
    public override void ExchangeTokens()
    {
        if (exchangeToken1_.GetComponent<SpriteRenderer>().sprite == lockSprite || exchangeToken2_.GetComponent<SpriteRenderer>().sprite == lockSprite) return;

        // Remember the starting and ending position for each token.
        Vector3 startPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos1_.x, (int)exchangeGridPos1_.y);
        Vector3 endPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos2_.x, (int)exchangeGridPos2_.y);

        // Get the new position for each token by lerping between startPos and endPos.
        Vector3 movePos1 = Vector3.Lerp(startPos, endPos, lerpPercent);
        Vector3 movePos2 = Vector3.Lerp(endPos, startPos, lerpPercent);

        // Update the position of each token object.
        exchangeToken1_.transform.position = movePos1;
        exchangeToken2_.transform.position = movePos2;

        // If both tokens have reached their final position:
        if (lerpPercent == 1)
        {
            // Tell the grid array the new positions of each token.
            gameManager.gridArray[(int)exchangeGridPos2_.x, (int)exchangeGridPos2_.y] = exchangeToken1_;
            gameManager.gridArray[(int)exchangeGridPos1_.x, (int)exchangeGridPos1_.y] = exchangeToken2_;

            // If this exchange has not created a match, move the tokens back.
            if (!matchManager.GridHasMatch() /*&& userSwap_*/)
            {
                exchangeToken1_.GetComponent<SpriteRenderer>().sprite = lockSprite;
                exchangeToken2_.GetComponent<SpriteRenderer>().sprite = lockSprite;
                points++;
                FindObjectOfType<Text>().text = points.ToString() + " points.";
                //SetupyTokenyExchangey(exchangeToken1_, exchangeGridPos2_, exchangeToken2_, exchangeGridPos1_, false);
            }

            // If this exchange has created a match, reset all variables and set movement to false.
            else
            {
                Debug.Log("FUCK YOU!!!");

                List<Rigidbody2D> rigidbodies = new List<Rigidbody2D>();
                
                foreach (SpriteRenderer spriteRenderer in FindObjectsOfType<SpriteRenderer>())
                {
                    spriteRenderer.gameObject.AddComponent<Rigidbody2D>();
                    rigidbodies.Add(spriteRenderer.GetComponent<Rigidbody2D>());
                }

                foreach(Rigidbody2D rigidybody in rigidbodies)
                {
                    rigidybody.AddForce(Random.insideUnitSphere * Random.Range(50f, 100f), ForceMode2D.Impulse);
                }

                exchangeToken1_ = null;
                exchangeToken2_ = null;
                move = false;
            }
        }
    }
}
