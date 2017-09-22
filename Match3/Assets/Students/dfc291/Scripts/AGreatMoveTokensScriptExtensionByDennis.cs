using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGreatMoveTokensScriptExtensionByDennis : MoveTokensScript {

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
}
