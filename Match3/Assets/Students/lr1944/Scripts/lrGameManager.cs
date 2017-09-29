using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lrGameManager : GameManagerScript {

	protected lrMatchManager lrMatchMan;



	public override void Start () {
		tokenTypes = (Object[])Resources.LoadAll("lr1944/Tokens/"); //grabbing prefabs
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

		token.transform.position = position + new Vector3 (0, 0, -10);

		RotateToken (token, x, y);

		if (x>1 && lrMatchMan.GridHasHorizontalMatch(x-2,y)) {
			rnd = (rnd + 1) % tokenTypes.Length;
			token.GetComponent<SpriteRenderer> ().sprite = (tokenTypes [rnd] as GameObject).GetComponent<SpriteRenderer> ().sprite;

		}

		if (y>1 && lrMatchMan.GridHasVerticalMatch(x,y-2)) {
			rnd = (rnd + 1) % tokenTypes.Length;
			token.GetComponent<SpriteRenderer> ().sprite = (tokenTypes [rnd] as GameObject).GetComponent<SpriteRenderer> ().sprite;
		}

		if (x>1 && lrMatchMan.GridHasHorizontalMatch(x-2,y)) {
			rnd = (rnd + 1) % tokenTypes.Length;
			token.GetComponent<SpriteRenderer> ().sprite = (tokenTypes [rnd] as GameObject).GetComponent<SpriteRenderer> ().sprite;

		}

	}

	//setting world position
	public virtual Vector2 GetWorldPositionFromGridPosition(int x, int y){
		return new Vector2(
			0,
			(y - gridHeight/2f) * tokenSize
		);
	}

	void RotateToken (GameObject token, int x, int y) {
		token.transform.RotateAround (new Vector3 (0,y,0), Vector3.up, (360f/gridWidth) * x);
		Debug.Log (x + " " + ((360f / gridWidth) * x));
	}


}
