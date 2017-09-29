using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lrMoveTokenBase : MonoBehaviour {

	// Access variables.
	protected GameManagerScript gameManager;
	protected MatchManagerScript matchManager;

	public bool move = false;   // Whether a token is currently moving.

	public float lerpPercent;
	public float lerpSpeed;

	protected bool userSwap;

	protected GameObject exchangeToken1;
	protected GameObject exchangeToken2;

	protected Vector2 exchangeGridPos1;
	protected Vector2 exchangeGridPos2;


	public virtual void Start () {
		// Assign references:
		gameManager = GetComponent<GameManagerScript>();
		matchManager = GetComponent<MatchManagerScript>();
		lerpPercent = 0;
	}


	public virtual void Update () {

		// If a token is currently moving, handle movement.
		if(move){

			// Increase lerp percent.
			lerpPercent += lerpSpeed;

			// Make sure lerp percent never exceeds 1 so that the lerp stops.
			if (lerpPercent >= 1){
				lerpPercent = 1;
			}

			// Make sure the exchange tokens are not null.
			if (exchangeToken1 != null){
				ExchangeTokens();
			}
		}
	}

	// Prepares variables to lerp movement.
	public void SetupTokenMove(){
		move = true;
		lerpPercent = 0;
	}


	// Recieves selected tokens from InputManagerScript and sets proper variables in preparation for the exchange.
	public void SetupTokenExchange(GameObject token1, Vector2 pos1,
		GameObject token2, Vector2 pos2, bool reversable){

		SetupTokenMove();

		// Set up references to each token object which will be swapped.
		exchangeToken1 = token1;
		exchangeToken2 = token2;

		// Sete up references to where each token will end up.
		exchangeGridPos1 = pos1;
		exchangeGridPos2 = pos2;

		this.userSwap = reversable;
	}


	// This function is responsible for moving the tokens and resetting the variables if there was a match, and putting them back into their previous positions
	// if there was not a match.
	public virtual void ExchangeTokens(){

		// Remember the starting and ending position for each token.
		Vector3 startPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos1.x, (int)exchangeGridPos1.y);
		Vector3 endPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos2.x, (int)exchangeGridPos2.y);

		// Get the new position for each token by lerping between startPos and endPos.
		Vector3 movePos1 = Vector3.Lerp(startPos, endPos, lerpPercent);
		Vector3 movePos2 = Vector3.Lerp(endPos, startPos, lerpPercent);

		// Update the position of each token object.
		exchangeToken1.transform.position = movePos1;
		exchangeToken2.transform.position = movePos2;

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


	// Moves a token downwards into an empty space.
	public virtual void MoveTokenToEmptyPos(int startGridX, int startGridY,
		int endGridX, int endGridY,
		GameObject token){

		// Get the starting and ending point for the upcoming move.
		Vector3 startPos = gameManager.GetWorldPositionFromGridPosition(startGridX, startGridY);
		Vector3 endPos = gameManager.GetWorldPositionFromGridPosition(endGridX, endGridY);

		// Get the new position of this token.
		Vector3 pos = Vector3.Lerp(startPos, endPos, lerpPercent);

		// Update the position of the token.
		token.transform.position =	pos;

		// If the token has reached its final position:
		if(lerpPercent == 1){
			// Tell the grid where the token is now, and set its previous space to empty.
			gameManager.gridArray[endGridX, endGridY] = token;
			gameManager.gridArray[startGridX, startGridY] = null;
		}
	}

	// Go through all tokens and see if their above an empty space, if so, call MoveTokenToEmptyPos() on it.
	// Returns true if all moves have finished.
	public virtual bool MoveTokensToFillEmptySpaces(){
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
					}
				}
			}
		}

		// If the tokens have reached their final position, stop moving them.
		if(lerpPercent == 1){
			move = false;
		}

		return movedToken;
	}
}
