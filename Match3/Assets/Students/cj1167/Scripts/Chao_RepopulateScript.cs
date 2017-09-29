using UnityEngine;
using System.Collections;

public class Chao_RepopulateScript : MonoBehaviour {
	
	protected Chao_GameManagerScript gameManager; //reference to game manager
	protected Chao_MatchManagerScript matchManager;

	public virtual void Start () {
		gameManager = GetComponent<Chao_GameManagerScript>(); //getting a refernece to GM component
		matchManager = GetComponent<Chao_MatchManagerScript>(); 
	}

	//Repopulate grid at the top
	public virtual void AddNewTokensToRepopulateGrid(){
		
		for(int x = 0; x < gameManager.gridWidth; x++){ //looping through the grid
			GameObject token = gameManager.gridArray[x, gameManager.gridHeight - 1]; //find token in position
			if(token == null){ //if there's no token there, add a token
				gameManager.AddTokenToPosInGrid(x, gameManager.gridHeight - 1, gameManager.grid);
			}
		}
		//Check if the grid has a match when repopulate it.
		while (matchManager.GridHasMatch()) {};
	}
}
