using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scGameManager : GameManagerScript {
    [SerializeField] ScoreManager scoreManager;
	public override void Start () {
		tokenTypes = (Object[])Resources.LoadAll("_Core/Tokens/"); //grabbing prefabs
		gridArray = new GameObject[gridWidth, gridHeight]; //creating the grid
		FixMakeGrid(); //populating the grid
		matchManager = GetComponent<MatchManagerScript>(); //assigning scripts to variables
		inputManager = GetComponent<InputManagerScript>();
		repopulateManager = GetComponent<RepopulateScript>();
		moveTokenManager = GetComponent<MoveTokensScript>();
	}
	public override void Update(){
		if(!GridHasEmpty()){ //if grid is fully populated
			if(matchManager.GridHasMatch()){ //if there are matches
				scoreManager.SetScore(matchManager.RemoveMatches()); //remove the matches
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
	protected void FixMakeGrid() {
		grid = new GameObject("TokenGrid");
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight; y++){
				AddTokenToPosInGrid(x, y, grid);
				while (IF_Match(x,y))
				{
					Destroy(gridArray[x,y]);
					gridArray[x,y] = null;
					AddTokenToPosInGrid(x, y, grid);
				}
			}
		}
	}

	bool IF_Match(int x, int y){
		Sprite token_sprite = gridArray[x,y].GetComponent<SpriteRenderer>().sprite;
		if(x > 0){
			if(token_sprite == gridArray[x-1,y].GetComponent<SpriteRenderer>().sprite){
				return true;
			}
		}
		if(y > 0){
			if(token_sprite == gridArray[x,y-1].GetComponent<SpriteRenderer>().sprite){
				return true;
			}
		}

		return false;
	}
}
