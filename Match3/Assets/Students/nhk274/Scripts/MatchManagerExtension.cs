using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManagerExtension : MatchManagerScript {

/*
	public override bool GridHasMatch ()
	{
		bool hasMatch = base.GridHasMatch ();

		//I PUT SOME CODE IN HERE TO CHECK FOR VERTICAL MATCHES
		// hasMatch = WHATEVER THIS SHOUD BE || hasMatch;      //this will return horizontal and vertical matches

		return hasMatch;

		//then, in Unity, instead of using MatchManagerScript, use MatchManagerExtended

		//also, look at the differences between the scenes in _Core
		//main difference is that DailySeedScene has a DailySeedGenerator script on there too, or something. Maybe check that out to see how it ties into what we did last week. 
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
} */

/* 
if I already found a match, leave match = true
else, check if we found a new match
match = match || GridHasHorizontalMatch(x, y);

if (match===1) {dont do anything}
else if (GridHasHorizontalMatch != null) {match=1} 
*/

protected SoundManager SM;
protected GameManagerExtended GM;

public override void Start ()
	{
		base.Start ();
		SM = GetComponent<SoundManager>();
		GM = GetComponent<GameManagerExtended>();
	}

//checking to see if there is a match anywhere on the grid
public override bool GridHasMatch(){
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
				if(y < gameManager.gridHeight - 2){
					match = match || GridHasVerticalMatch(x, y);
				}
			}
		}
		return match;
	}

//checking for horizontal matches in the entire grid
	public bool GridHasVerticalMatch(int x, int y){
        //reference to 3 game objects along y row, starting from x
        //deciding which tokens to use to check matches
		GameObject token1 = gameManager.gridArray[x, y+0];
		GameObject token2 = gameManager.gridArray[x, y+1];
		GameObject token3 = gameManager.gridArray[x, y+2];
		
		if(token1 != null && token2 != null && token3 != null){
            //after null check, evaluate match among the three sprites
			SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
			SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
			SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();

			//this is returning either true or false cause it's returning either 0 or 1
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
		//GM.score += 100;
		SM.source2.clip = SM.match;
		SM.RandomizeVolandPitch(SM.source2);
		SM.source2.Play();
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

			if(y < gameManager.gridHeight - 2){

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