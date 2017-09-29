using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lrRepopulate : RepopulateScript {

	public override void AddNewTokensToRepopulateGrid() {
		for(int x = 0; x < gameManager.gridWidth; x++){ //looping through the grid
			GameObject token = gameManager.gridArray[x, gameManager.gridHeight-1]; //find token in position
			if(token == null){ //if there's no token there, add a token
				((lrGameManager)gameManager).AddTokenToPosInGridNew(x, gameManager.gridHeight - 1, gameManager.grid);
			}
		}
	}
}
