using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pao_GameManager : GameManagerScript {

	public override void Start(){
		tokenTypes = (Object[])Resources.LoadAll("_Core/Tokens/"); //grabbing prefabs
		gridArray = new GameObject[gridWidth, gridHeight]; //creating the grid
		MakeGridNoMatch();
 		matchManager = GetComponent<MatchManagerScript>(); //assigning scripts to variables
		inputManager = GetComponent<InputManagerScript>();
		repopulateManager = GetComponent<RepopulateScript>();
		moveTokenManager = GetComponent<MoveTokensScript>();

	}

	void MakeGridNoMatch(){
		grid = new GameObject("TokenGrid");
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight; y++){
				AddTokenToPosInGrid(x, y, grid);
				while(TileHasMatch(x,y)){
					Destroy(gridArray[x,y]);
					gridArray[x,y] = null;
					AddTokenToPosInGrid(x,y,grid);					
				} 
				// while(TileHasMatch(x,y)){
				// }
				
			}
		}
	}

	public bool TileHasMatch(int x_, int y_){
		Sprite mySprite = gridArray[x_,y_].GetComponent<SpriteRenderer>().sprite;

		if(y_>0){
			if(mySprite == gridArray [x_,y_-1].GetComponent<SpriteRenderer>().sprite){
				return true;
			}
		}

		if(x_>0){
			if(mySprite == gridArray[x_-1, y_].GetComponent<SpriteRenderer>().sprite){
				return true;
			}
	
		}
		return false;
	} 

	public void DetectGiants(){
		
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight; y++){
				Sprite mySprite = gridArray[x,y].GetComponent<SpriteRenderer>().sprite;
				if (mySprite.name == "giants"){
					GameObject giantsToken = gridArray[x,y];
					GameObject giantCollectible = GameObject.CreatePrimitive(PrimitiveType.Cube);
					giantCollectible.transform.position = gridArray[x,y].transform.position;
					giantCollectible.GetComponent<MeshRenderer>().enabled = false;
					giantCollectible.AddComponent<Pao_GiantsCollectible>();
					giantCollectible.GetComponent<Pao_GiantsCollectible>().GetGiants(giantsToken);
 				}	
			}
		}
	}

}
