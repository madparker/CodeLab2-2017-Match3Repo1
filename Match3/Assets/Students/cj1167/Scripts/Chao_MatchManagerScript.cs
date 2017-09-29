using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chao_MatchManagerScript : MonoBehaviour {

	protected Chao_GameManagerScript gameManager;
	public int Score;
	public GameObject ScoreText;

	public virtual void Start (){
		
		gameManager = GetComponent<Chao_GameManagerScript>();

	}

	//check if the grid has match, if it does, remove them
	public bool GridHasMatch (){
		
		//a list to put all the removeable tokens
		List<int[]> toBeRemovedTotal = new List<int[]>();

		//check if the whole grid has match, if it does, put them in the list
		for(int x = 0; x < gameManager.gridWidth ; x++){
			
			for(int y = 0; y < gameManager.gridHeight ; y++){

				foreach (var token in GridMatch (x,y)) {
					toBeRemovedTotal.Add (token);
				}

			}
		}

		//if there are tokens in the list, remove them.
		if (toBeRemovedTotal.Count > 0) {
			//for each token in the list, remove them by send the (x,y) to the removematches
			foreach (var i in toBeRemovedTotal) {
				RemoveMatches (i [0], i [1]);
			}

			Score = Score + toBeRemovedTotal.Count;
			ScoreText.gameObject.GetComponent<TextMesh> ().text = "Score: " + Score;
		}

		//return true if it has match
		return toBeRemovedTotal.Count > 0;

	}

	//GridMatch return as a list
	public List<int[]> GridMatch(int x, int y) {
			
		//a list of the tokens that have a match
		List<int[]> toBeRemoved = new List<int[]>();
		GameObject token1 = gameManager.gridArray[x, y];
		GameObject token2;
		//Check how many tokens above match with it
		int Upmatch = 0;
		for(int i = 1; i< gameManager.gridHeight - y ; i++){
			token2  = gameManager.gridArray[x, y + i];

			if (token1 != null && token2 != null) {

				if (string.Equals(token1.gameObject.tag, token2.gameObject.tag)) {
//					Debug.Log ("token1:" + token1.gameObject.tag);
//					Debug.Log ("token2:" + token2.gameObject.tag);
					
					Upmatch = i;
				} 
				else {
				
					break;
				}
			}

		}
		//Check how many tokens under match with it
		int Downmatch = 0;
		for (int i = 1; i < y + 1; i++) {

		token2 = gameManager.gridArray [x, y - i];

		if (token1 != null && token2 != null) {
			if (string.Equals (token1.gameObject.tag, token2.gameObject.tag)) {
			
				Downmatch = i;
			} else {
				break;
			}
		}
	}
		//Check how many tokens on left match with it
		int Leftmatch = 0;
		for(int i = 1; i< x + 1; i++){

			token2 = gameManager.gridArray[x - i, y];

			if (token1 != null && token2 != null) {
				if (string.Equals (token1.gameObject.tag, token2.gameObject.tag)) {
					Leftmatch = i;
				} else {
					break;
				}
			}
		}
		//Check how many tokens on left match with it
		int Rightmatch = 0;
		for(int i = 1; i< gameManager.gridWidth-x; i++){

			token2 = gameManager.gridArray[x + i, y];

			if (token1 != null && token2 != null) {
				if (string.Equals (token1.gameObject.tag, token2.gameObject.tag)) {
					Rightmatch = i;
				} else {
					break;
				}
			}
		}
			
	
		//if match more than 2, then add them to the list
		if ((Upmatch + Downmatch) > 1) {	

			for (int i = 0; i <= Upmatch + Downmatch; i++) {
//					RemoveMatches (x, y - Downmatch + i);
			toBeRemoved.Add(new int[]{x, y - Downmatch + i});
			}

		}
		//if match more than 2, then add them to the list
		if ((Leftmatch + Rightmatch) > 1) {
//			Debug.Log (Leftmatch);
//			Debug.Log (Rightmatch);
			for (int i = 0; i <= Leftmatch + Rightmatch; i++) {
//					RemoveMatches (x -Leftmatch + i, y);
			toBeRemoved.Add(new int[]{x -Leftmatch + i, y});
			}
			
		}
		//return the list	
		return toBeRemoved;

	}
		

	public void RemoveMatches (int x, int y){


		GameObject token = gameManager.gridArray[x, y]; 

		if(token != null){
			Destroy(token);
		}

		gameManager.gridArray[x, y] = null;

	}


}
