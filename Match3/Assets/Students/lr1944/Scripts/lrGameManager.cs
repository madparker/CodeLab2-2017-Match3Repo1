using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lrGameManager : GameManagerScript {

	protected lrMatchManager lrMatchMan;



	public override void Start () {
		tokenTypes = (Object[])Resources.LoadAll("_Core/Tokens/"); //grabbing prefabs
		gridArray = new GameObject[gridWidth, gridHeight]; //creating the grid

		matchManager = GetComponent<MatchManagerScript>(); //assigning scripts to variables
		inputManager = GetComponent<InputManagerScript>();
		repopulateManager = GetComponent<RepopulateScript>();
		moveTokenManager = GetComponent<MoveTokensScript>();
		lrMatchMan = GetComponent<lrMatchManager> ();

		MakeGridNew(); //populating the grid


	}
	


	void MakeGridNew() {
		grid = new GameObject("TokenGrid");
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight; y++){
				AddTokenToPosInGridNew(x, y, grid);


			}
		}
	}


	//adds random token types to specific positions in the grid
	public void AddTokenToPosInGridNew(int x, int y, GameObject parent){
		Vector3 position = GetWorldPositionFromGridPosition(x, y); 

		int rnd = Random.Range (0, tokenTypes.Length);
			
		GameObject token = 
			//grabbing random token types
			Instantiate(tokenTypes[rnd], position, Quaternion.identity) as GameObject;
		token.transform.parent = parent.transform;
		gridArray[x, y] = token;

		if (x>1 && lrMatchMan.GridHasHorizontalMatch(x-2,y)) {
			rnd = (rnd + 1) % tokenTypes.Length;
			token.GetComponent<SpriteRenderer> ().sprite = (tokenTypes [rnd] as GameObject).GetComponent<SpriteRenderer> ().sprite;

		}

		if (y>1 && lrMatchMan.GridHasVerticalMatch(x,y-2)) {
			rnd = (rnd + 1) % tokenTypes.Length;
			token.GetComponent<SpriteRenderer> ().sprite = (tokenTypes [rnd] as GameObject).GetComponent<SpriteRenderer> ().sprite;
		}

//		if (x > 1) {
//			
//			for (lrMatchMan.GridHasHorizontalMatch(x,y)) {
//				Destroy (token);
//				gridArray [x, y] = null;
//				GameObject tokenNew = 
//					//grabbing random token types
//					Instantiate(tokenTypes[Random.Range(0, tokenTypes.Length)], 
//						position, 
//						Quaternion.identity) as GameObject;
//				token.transform.parent = parent.transform;
//				gridArray[x, y] = tokenNew;
//			}
//			
//		}
//
//		if (y > 1) {
//			for (lrMatchMan.GridHasVerticalMatch(x,y)) {
//				Destroy (token);
//				gridArray [x, y] = null;
//				GameObject tokenNew = 
//					//grabbing random token types
//					Instantiate(tokenTypes[Random.Range(0, tokenTypes.Length)], 
//						position, 
//						Quaternion.identity) as GameObject;
//				token.transform.parent = parent.transform;
//				gridArray[x, y] = tokenNew;
//			}
//		}


	}


}
