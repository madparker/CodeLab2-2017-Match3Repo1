using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	public int gridWidth = 8;
	public int gridHeight = 8;
	public float tokenSize = 1;

	//references to other managers
	//these can only be accessed by code in this class, or class derived from this class
	protected MatchManagerScript matchManager;
	protected InputManagerScript inputManager;
	protected RepopulateScript repopulateManager;
	protected MoveTokensScript moveTokenManager;

	public GameObject grid;  
	public  GameObject[,] gridArray; //2D array for grid
	protected Object[] tokenTypes;
	GameObject selected;

	//can be overridden
	public virtual void Start () {
		tokenTypes = (Object[])Resources.LoadAll("_Core/Tokens/"); //grabbing prefabs
		gridArray = new GameObject[gridWidth, gridHeight]; //creating the grid
		MakeGrid(); //populating the grid
		matchManager = GetComponent<MatchManagerScript>(); //assigning scripts to variables
		inputManager = GetComponent<InputManagerScript>();
		repopulateManager = GetComponent<RepopulateScript>();
		moveTokenManager = GetComponent<MoveTokensScript>();
	}

	public virtual void Update(){
		if(!GridHasEmpty()){ //if grid is fully populated
			if(matchManager.GridHasMatch()){ //if there are matches
				matchManager.RemoveMatches(); //remove the matches
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

	//creates grid and populates it with random tokens
	void MakeGrid() {
		grid = new GameObject("TokenGrid");
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight; y++){
				AddTokenToPosInGrid(x, y, grid);
			}
		}
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
