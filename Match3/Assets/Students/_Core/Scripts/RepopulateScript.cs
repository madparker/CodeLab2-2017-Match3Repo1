using UnityEngine;
using System.Collections;

public class RepopulateScript : MonoBehaviour {
	
	protected GameManagerScript gameManager; //reference to game manager

	public virtual void Start () {
		gameManager = GetComponent<GameManagerScript>(); //getting a refernece to GM component
	}

	//Repopulate grid at the top
	public virtual void AddNewTokensToRepopulateGrid(){
		for(int x = 0; x < gameManager.gridWidth; x++){ //looping through the grid
			GameObject token = gameManager.gridArray[x, gameManager.gridHeight - 1]; //find token in position
			if(token == null){ //if there's no token there, add a token
				gameManager.AddTokenToPosInGrid(x, gameManager.gridHeight - 1, gameManager.grid);
			}
		}
	}
}
