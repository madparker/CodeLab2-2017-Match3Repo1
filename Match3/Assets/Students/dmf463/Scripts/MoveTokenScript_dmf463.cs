using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTokenScript_dmf463 : MoveTokensScript {

    public float duration;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        // If a token is currently moving, handle movement.
        if (move)
        {
            // Increase lerp percent.
            lerpPercent += lerpSpeed;

            // Make sure lerp percent never exceeds 1 so that the lerp stops.
            if (lerpPercent >= duration)
            {
                lerpPercent = duration;
            }
            // Make sure the exchange tokens are not null.
            if (exchangeToken1 != null)
            {
                ExchangeTokens();
            }
        }
    }

    public override void ExchangeTokens()
    {
        base.ExchangeTokens();
    }

    public override bool MoveTokensToFillEmptySpaces()
    {
        bool movedToken = false;

        for (int x = 0; x < gameManager.gridWidth; x++)
        {
            for (int y = 1; y < gameManager.gridHeight; y++)
            {
                if (gameManager.gridArray[x, y - 1] == null)
                {
                    for (int pos = y; pos < gameManager.gridHeight; pos++)
                    {
                        GameObject token = gameManager.gridArray[x, pos];
                        //something about them all falling together is making it so that the movement isn't smooth
                        //when it falls one by one, the lerping is perfect.
                        //perhaps they're sharing a value?
                        //why would they skip one...
                        if (token != null)
                        {
                            MoveTokenToEmptyPos(x, pos, x, pos - 1, token);
                            movedToken = true;
                        }
                    }
                }
            }
        }
        // If the tokens have reached their final position, stop moving them.
        if (lerpPercent >= duration)
        {
            move = false;
        }
        return movedToken;
    }

    public override void MoveTokenToEmptyPos(int startGridX, int startGridY, int endGridX, int endGridY, GameObject token)
    {
        // Get the starting and ending point for the upcoming move.
        Vector3 startPos = gameManager.GetWorldPositionFromGridPosition(startGridX, startGridY);
        Vector3 endPos = gameManager.GetWorldPositionFromGridPosition(endGridX, endGridY);

        // Get the new position of this token.
        Vector3 pos = Vector3.Lerp(startPos, endPos, lerpPercent);

        // Update the position of the token.
        token.transform.position = pos;

        // If the token has reached its final position:
        if (lerpPercent >= duration)
        {
            // Tell the grid where the token is now, and set its previous space to empty.
            gameManager.gridArray[endGridX, endGridY] = token;
            gameManager.gridArray[startGridX, startGridY] = null;
        }
    }

}
