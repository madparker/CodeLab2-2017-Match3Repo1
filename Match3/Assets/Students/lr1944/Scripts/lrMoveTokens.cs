using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lrMoveTokens : MoveTokensScript {

	public override bool MoveTokensToFillEmptySpaces(){
		bool movedToken = false;

		for(int x = 0; x < gameManager.gridWidth; x++){
			for(int y = 1; y < gameManager.gridHeight ; y++){
				if(gameManager.gridArray[x, y - 1] == null){
					for(int pos = y; pos < gameManager.gridHeight; pos++){
						GameObject token = gameManager.gridArray[x, pos];
						if(token != null){
							MoveTokenToEmptyPos(x, pos, x, pos - 1, token);
							movedToken = true;
						}
						break;
					}
				}

			}
//			break;
		}

		// If the tokens have reached their final position, stop moving them.
		if(lerpPercent == 1){
			move = false;
		}

		return movedToken;
	}

	public override void ExchangeTokens(){

		// Remember the starting and ending position for each token.
		Vector3 startPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos1.x, (int)exchangeGridPos1.y);
		Vector3 endPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos2.x, (int)exchangeGridPos2.y);

		// Get the new position for each token by lerping between startPos and endPos.
		Vector3 movePos1 = Vector3.Lerp(startPos, endPos, lerpPercent);
		Vector3 movePos2 = Vector3.Lerp(endPos, startPos, lerpPercent);



		// Update the position of each token object.
		exchangeToken1.transform.position = movePos1;

		((lrGameManager)gameManager).RotateToken (exchangeToken1);

		exchangeToken2.transform.position = movePos2;

		((lrGameManager)gameManager).RotateToken (exchangeToken2);



		// If both tokens have reached their final position:
		if(lerpPercent == 1){

			// Tell the grid array the new positions of each token.
			gameManager.gridArray[(int)exchangeGridPos2.x, (int)exchangeGridPos2.y] = exchangeToken1;
			gameManager.gridArray[(int)exchangeGridPos1.x, (int)exchangeGridPos1.y] = exchangeToken2;

			// If this exchange has not created a match, move the tokens back.
			if(!matchManager.GridHasMatch() && userSwap){
				SetupTokenExchange(exchangeToken1, exchangeGridPos2, exchangeToken2, exchangeGridPos1, false);

				// If this exchange has created a match, reset all variables and set movement to false.
			} else {
				exchangeToken1 = null;
				exchangeToken2 = null;
				move = false;
			}
		}
	}


	public override void MoveTokenToEmptyPos(int startGridX, int startGridY,
		int endGridX, int endGridY,
		GameObject token){

		// Get the starting and ending point for the upcoming move.
		Vector3 startPos = gameManager.GetWorldPositionFromGridPosition(startGridX, startGridY);
		Vector3 endPos = gameManager.GetWorldPositionFromGridPosition(endGridX, endGridY);

		// Get the new position of this token.
		Vector3 pos = Vector3.Lerp(startPos, endPos, lerpPercent);

		// Update the position of the token.
		token.transform.position =	pos;

		((lrGameManager)gameManager).RotateToken (token);

		// If the token has reached its final position:
		if(lerpPercent == 1){
			// Tell the grid where the token is now, and set its previous space to empty.
			gameManager.gridArray[endGridX, endGridY] = token;
			gameManager.gridArray[startGridX, startGridY] = null;
		}
	}





}
