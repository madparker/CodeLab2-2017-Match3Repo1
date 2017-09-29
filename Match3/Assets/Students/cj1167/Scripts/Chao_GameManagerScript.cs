using UnityEngine;
using System.Collections;

public class Chao_GameManagerScript : MonoBehaviour {

	public int gridWidth = 8;
	public int gridHeight = 8;
	public float tokenSize = 1;

	//references to other managers
	//these can only be accessed by code in this class, or class derived from this class
	protected Chao_MatchManagerScript matchManager;
	protected Chao_InputManagerScript inputManager;
	protected Chao_RepopulateScript repopulateManager;
	protected Chao_MoveTokensScript moveTokenManager;

	public GameObject grid;  
	public  GameObject[,] gridArray; //2D array for grid
	//Change it to public and put the tokens in inspector.
	public GameObject[] tokenTypes;
	GameObject selected;

	public virtual void Start () {
		
		//tokenTypes = (Object[])Resources.LoadAll("_Core/Tokens/"); //grabbing prefabs
		gridArray = new GameObject[gridWidth, gridHeight]; //creating the grid

		matchManager = GetComponent<Chao_MatchManagerScript>(); 
		inputManager = GetComponent<Chao_InputManagerScript>();
		repopulateManager = GetComponent<Chao_RepopulateScript>();
		moveTokenManager = GetComponent<Chao_MoveTokensScript>();

		MakeGrid(); //populating the grid}
		moveTokenManager.StepText.gameObject.GetComponent<TextMesh> ().text = "Step: " + moveTokenManager.Step;
	}

	public virtual void Update(){
		
		//if grid doesn't have any empty space, all match to happen
		if(!GridHasEmpty()){ 
			//if match happen, remove them.
//			if(matchManager.GridHasMatch()){ 
//				
//				matchManager.RemoveMatches(); 
//			} 
			//if there is no match, then allow player select token to make match happen.
//			else {
				inputManager.SelectToken(); 
//			}

		} 
		//if the grid has empty space
		else { 
			//if the grid has empty space because the old tokens are not moving to fill the space, let them start to move. 
			if(!moveTokenManager.move){ 
				moveTokenManager.SetupTokenMove(); 
			}
			//if the grid has empty space becasue it need new tokens, tell it to make new tokens.
			if(!moveTokenManager.MoveTokensToFillEmptySpaces()){ 
				repopulateManager.AddNewTokensToRepopulateGrid(); 
			}
		}
	}

	//creates grid and populates it with random tokens
	void MakeGrid() {
		
		grid = new GameObject("TokenGrid");

		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight; y++){
				AddTokenToPosInGrid(x, y, grid);
			}
		}

		matchManager.GridHasMatch ();


	}

	//Cheks for empty space in grid. If there is an empty space, returns true, otherwise, false.
	public virtual bool GridHasEmpty(){
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight ; y++){
				if(gridArray[x, y] == null){
					return true;
				}
			}
		}

		return false;
	}

	//returns Vector2 position of given token
	public Vector2 GetPositionOfTokenInGrid(GameObject token){
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight ; y++){
				if(gridArray[x, y] == token){
					return(new Vector2(x, y));
				}
			}
		}
		return new Vector2();
	}

	//setting world position
	public Vector2 GetWorldPositionFromGridPosition(int x, int y){
		return new Vector2(
			(x - gridWidth/2) * tokenSize,
			(y - gridHeight/2) * tokenSize);
	}

	//adds random token types to specific positions in the grid
	public void AddTokenToPosInGrid(int x, int y, GameObject parent){
		Vector3 position = GetWorldPositionFromGridPosition(x, y); 
		GameObject token = 
			//grabbing random token types
			Instantiate(tokenTypes[Random.Range(0, tokenTypes.Length)], 
			            position, 
			            Quaternion.identity) as GameObject;
		token.transform.parent = parent.transform;
		gridArray[x, y] = token;
	}
}
