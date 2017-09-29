using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerExtended : GameManagerScript {

	//ScoreParameters
	public int score = 0;
	public float resetDelay = 1f;
	public GameObject levelNumber;

	public override void Start ()
	{
		base.Start ();
		score = 0;
	}

	public override void Update () {
		if(!GridHasEmpty()){ //if grid is fully populated
				if(matchManager.GridHasMatch()){ //if there are matches
					//matchManager.RemoveMatches();
					if (matchManager.RemoveMatches() > 0) {score += 100;} //remove the matches
				} else {
					inputManager.SelectToken(); //allow token to be selected
				}
			} else { //if grid not fully populated
				if(!moveTokenManager.move){ //if token movement is false
					moveTokenManager.SetupTokenMove(); //set it true so they can fill empty space
				}
				if(!moveTokenManager.MoveTokensToFillEmptySpaces()){ //if is false
					repopulateManager.AddNewTokensToRepopulateGrid(); //and there are still free spaces, allow it to populate
				}
			}
}
}
