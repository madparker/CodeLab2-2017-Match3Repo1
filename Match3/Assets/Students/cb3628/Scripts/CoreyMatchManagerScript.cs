using UnityEngine;
using System.Collections;

public class CoreyMatchManagerScript : MonoBehaviour {
    /// <summary>
    /// This script runs checks on horizontal and vertical matches
    /// seems to only check for horizontal matches
    /// </summary>

	protected CoreyGameManagerScript gameManager;


	public virtual void Start () {
		gameManager = GetComponent<CoreyGameManagerScript>();
	}

    //checking to see if there is a match anywhere on the grid
	public virtual bool GridHasMatch(){
		bool match = false;
		
        //cycle through columns (x position)
		for(int x = 0; x < gameManager.gridWidth; x++){
            //cycle through horizontal elements in column
			for(int y = 0; y < gameManager.gridHeight ; y++){
                //don't need to evaluate last two rows, because 
                //GridHasHorizontalMatch method is checking last two
				if(x < gameManager.gridWidth - 2){
                    //once we've found a match, match will always return true
					match = match || GridHasHorizontalMatch(x, y);
				}
			}
		}
		
		return match;
	}

    //checking for horizontal matches in the entire grid
	public bool GridHasHorizontalMatch(int x, int y){
        //reference to 3 game objects along y row, starting from x
		GameObject token1 = gameManager.gridArray[x + 0, y];
		GameObject token2 = gameManager.gridArray[x + 1, y];
		GameObject token3 = gameManager.gridArray[x + 2, y];
		
		if(token1 != null && token2 != null && token3 != null){
            //after null check, evaluate match among the three sprites
			SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
			SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
			SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();
			
			return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
		} else {
			return false;
		}
	}


	public int GetHorizontalMatchLength(int x, int y){
		int matchLength = 1;
		
		GameObject first = gameManager.gridArray[x, y];

		if(first != null){
            //null check, then get first sprite
			SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();
			
            //evaluate along subsequent columns in this row to get match length
			for(int i = x + 1; i < gameManager.gridWidth; i++){
				GameObject other = gameManager.gridArray[i, y];

				if(other != null){
					SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

					if(sr1.sprite == sr2.sprite){
						matchLength++;
					} else {
                        //break out of loop if sprites inequal...
						break;
					}
				} else {
                    //break immediately if null...
					break;
				}
			}
		}
		//... then return the length of the match
		return matchLength;
	}

	public virtual int RemoveMatches(){
		int numRemoved = 0;

        //cycle through column first... 
		for(int x = 0; x < gameManager.gridWidth; x++){
            //then row...
			for(int y = 0; y < gameManager.gridHeight ; y++){

                //stop at column(gridWidth - 2) because this is minimum column
                //to check for 3-sprite match
				if(x < gameManager.gridWidth - 2){

                    //Call GetHorizontalMatchLength to get the length of the match
					int horizonMatchLength = GetHorizontalMatchLength(x, y);

                    //Match must be 3 or more...
					if(horizonMatchLength > 2){
                        
                        //...to go through and delete each sprite in this match
						for(int i = x; i < x + horizonMatchLength; i++){
							GameObject token = gameManager.gridArray[i, y]; 
							Destroy(token);

							gameManager.gridArray[i, y] = null;
                            //record number of items removed
							numRemoved++;
						}
					}
				}
			}
		}
		
		return numRemoved;
	}
}
