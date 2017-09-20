using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManagerScriptVertical : MatchManagerScript {

	public override bool GridHasMatch(){
		bool match = false;
		//first we check if the original script gave us any matches
		if (!base.GridHasMatch()){
		//if not, do what we did for horizontal matches, cycling through grid 
			for(int x = 0; x < gameManager.gridWidth; x++){
				for(int y = 0; y < gameManager.gridHeight ; y++){
			//changed these lines to work for vertical checking, reference new vert bool
					if(y < gameManager.gridHeight - 2){
						match = match || GridHasVerticalMatch(x, y);
					}
				}
			}
		} else { 
			return base.GridHasMatch();
		}
		return match;
	}

	public bool GridHasVerticalMatch(int x, int y){
		//reference to 3 game objects along y row, starting from x
		GameObject token1 = gameManager.gridArray[x, y + 0];
		GameObject token2 = gameManager.gridArray[x, y + 1];
		GameObject token3 = gameManager.gridArray[x, y + 2];

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
			for(int i = y + 1; i < gameManager.gridHeight; i++){
				GameObject other = gameManager.gridArray[x,i];

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

	public override int RemoveMatches()
	{
		int vertNumRemoved = 0;

		//cycle through column first... 
		for(int x = 0; x < gameManager.gridWidth; x++){
			//then row...
			for(int y = 0; y < gameManager.gridHeight ; y++){

				//stop at row(gridHeight - 2) because this is minimum row
				//to check for 3-sprite match
				if(y < gameManager.gridHeight - 2){

					//Call GetVerticalMatchLength to get the length of the match
					int verticalMatchLength = GetVerticalMatchLength(x, y);

					//Match must be 3 or more...
					if(verticalMatchLength > 2){

						//...to go through and delete each sprite in this match
						for(int i = y; i < y + verticalMatchLength; i++){
							GameObject token = gameManager.gridArray[x, i]; 
							Destroy(token);

							gameManager.gridArray[x, i] = null;
							//record number of items removed
							vertNumRemoved++;
						}
					}
				}
			}
		}
		if(vertNumRemoved < 0){
			return vertNumRemoved;
		} else{
			return base.RemoveMatches();
		}
	}
}
