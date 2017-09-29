using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMatchManagerScript : MatchManagerScript {

	// Use this for initialization

    public override void Start() {
        base.Start();
    }


    public override bool GridHasMatch() {
        //check for horizontal matches first?
        //can't just call base method, because we're setting base to false
        bool horizMatch = base.GridHasMatch();
        if (!horizMatch) {
            //This checks for vertical matches in grid
            bool vertMatch = false;

            //cycle through columns (y position)
            for (int x = 0; x < gameManager.gridHeight; x++) {
                //cycle through horizontal elements in column
                for (int y = 0; y < gameManager.gridWidth; y++) {
                    //don't need to evaluate last two rows, because 
                    //GridHasHorizontalMatch method is checking last two
                    if (y < gameManager.gridHeight - 2) {
                        //once we've found a match, match will always return true
                        vertMatch = vertMatch || GridHasVerticalMatch(x, y);
                    }
                }
            }

            return vertMatch;
        }
        else return horizMatch;
	}

	//checking for vertical matches
	public bool GridHasVerticalMatch(int x, int y) {
		//reference to 3 game objects along y row, starting from y
        GameObject token1 = gameManager.gridArray[x, y + 0];
        GameObject token2 = gameManager.gridArray[x, y + 1];
        GameObject token3 = gameManager.gridArray[x, y + 2];

		if (token1 != null && token2 != null && token3 != null) {
			//after null check, evaluate match among the three sprites
			SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
			SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
			SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();

			return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
		}
		else {
			return false;
		}
	}


	public int GetVerticalMatchLength(int x, int y) {
		int matchLength = 1;

		GameObject first = gameManager.gridArray[x, y];

		if (first != null) {
			//null check, then get first sprite
			SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();

			//evaluate along subsequent columns in this column to get match length
            for (int i = y + 1; i < gameManager.gridHeight; i++) {
                GameObject other = gameManager.gridArray[x, i];

				if (other != null) {
					SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

					if (sr1.sprite == sr2.sprite) {
						matchLength++;
					}
					else {
						//break out of loop if sprites inequal...
						break;
					}
				}
				else {
					//break immediately if null...
					break;
				}
			}
		}
		//... then return the length of the match
		return matchLength;
	}

    public override int RemoveMatches() {
        //perform base function first
        int horizRemoved = base.RemoveMatches();

        if (horizRemoved <= 0) {
            int vertRemoved = 0;

            //cycle through row first... 
            for (int y = 0; y < gameManager.gridHeight; y++) {
                //then column...
                for (int x = 0; x < gameManager.gridWidth; x++) {

                    //stop at column(gridWidth - 2) because this is minimum column
                    //to check for 3-sprite match
                    if (y < gameManager.gridHeight - 2) {

                        //Call GetVerticalMatchLength to get the length of the match
                        int verticalMatchLength = GetVerticalMatchLength(x, y);

                        //Match must be 3 or more...
                        if (verticalMatchLength > 2) {

                            //...to go through and delete each sprite in this match
                            for (int i = y; i < y + verticalMatchLength; i++) {
                                GameObject token = gameManager.gridArray[x, i];
                                Destroy(token);

                                gameManager.gridArray[x, i] = null;
                                //record number of items removed
                                vertRemoved++;
                            }
                        }
                    }
                }
            }


            return vertRemoved;
        }
        else return horizRemoved;
	}
}
