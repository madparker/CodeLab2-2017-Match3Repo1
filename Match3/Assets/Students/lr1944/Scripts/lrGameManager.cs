using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lrGameManager : GameManagerScript {

	protected lrMatchManager lrMatchMan;

	public override void Start () {
		tokenTypes = (Object[])Resources.LoadAll("_Core/Tokens/"); //grabbing prefabs
		gridArray = new GameObject[gridWidth, gridHeight]; //creating the grid
		MakeGrid(); //populating the grid
		matchManager = GetComponent<MatchManagerScript>(); //assigning scripts to variables
		inputManager = GetComponent<InputManagerScript>();
		repopulateManager = GetComponent<RepopulateScript>();
		moveTokenManager = GetComponent<MoveTokensScript>();
		lrMatchMan = GetComponent<lrMatchManager> ();
	}
	

	public override void Update(){
		if(!GridHasEmpty()){ //if grid is fully populated
			if(lrMatchMan.GridHasMatch()){ //if there are matches
				lrMatchMan.RemoveMatches(); //remove the matches
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

	void MakeGrid() {
		grid = new GameObject("TokenGrid");
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight; y++){
				AddTokenToPosInGrid(x, y, grid);
			}
		}
	}
}
