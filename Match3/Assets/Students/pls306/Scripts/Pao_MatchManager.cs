using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pao_MatchManager : MatchManagerScript {

	public override void Start(){
		base.Start();
	}
	public override bool GridHasMatch ()
	{
		bool hasMatch = false;
 		
		for (int x = 0; x < gameManager.gridWidth; x++)
        {
            //cycle through horizontal elements in column
            for (int y = 0; y < gameManager.gridHeight; y++)
            {
                //don't need to evaluate last two rows, because 
                //GridHasHorizontalMatch method is checking last two
                if (x < gameManager.gridWidth - 2)
                {
                    //once we've found a match, match will always return true
                    hasMatch = hasMatch || GridHasHorizontalMatch(x, y);
                }
               if (y < gameManager.gridHeight - 2) {
                    hasMatch = hasMatch || GridHasVerticalMatch(x, y);
                }

				// if (x < gameManager.gridWidth - 2 && y < gameManager.gridHeight -2)
                // {
                //     //once we've found a match, match will always return true
                //     hasMatch = hasMatch || GridHasHorizontalMatch(x, y) || GridHasVerticalMatch(x,y);
                // }            
			}
        }

 
		return hasMatch;
	}

	public bool GridHasVerticalMatch(int x, int y){
        //reference to 3 game objects along y row, starting from x
		GameObject token1 = gameManager.gridArray[x, y];
		GameObject token2 = gameManager.gridArray[x, y+1];
		GameObject token3 = gameManager.gridArray[x, y+2];
		
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

	public int GetVerticalMatchLength(int x, int y){
		int matchLength = 1;
		
		GameObject first = gameManager.gridArray[x, y];

		if(first != null){
            //null check, then get first sprite
			SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();
			
            //evaluate along subsequent columns in this row to get match length
			for(int i = y + 1; i < gameManager.gridWidth; i++){
				GameObject other = gameManager.gridArray[x, i];

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

	public override int RemoveMatches(){
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

				if (y < gameManager.gridHeight - 2){
					 //Call GetHorizontalMatchLength to get the length of the match
					int verticalMatchLength = GetVerticalMatchLength(x, y);

                    //Match must be 3 or more...
					if(verticalMatchLength > 2){
                        
                        //...to go through and delete each sprite in this match
						for(int i = y; i < y + verticalMatchLength; i++){
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

 }
