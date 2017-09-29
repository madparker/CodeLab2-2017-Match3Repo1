using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pao_ScoreManager : MonoBehaviour {

	public static int score;
	protected Pao_GameManager gameManager;

	private const float COROUTINE_DELAY = 2f; 

	public enum GameState {
		MATCH_3,
		PLATFORMER
	}
	public GameState gameState;

	public float timeLimit;
	void Start(){
		gameManager = GetComponent<Pao_GameManager>();
		gameState = GameState.MATCH_3;
	}


	void Update(){
		switch(gameState){
			case GameState.MATCH_3:
				timeLimit -= Time.deltaTime;
				if(timeLimit <= 0){
					GameObject character = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					character.transform.position = gameManager.gridArray[gameManager.gridWidth/2, gameManager.gridHeight-2].transform.position + (Vector3.up * 2); 
					// Destroy(character.GetComponent<BoxCollider>());
					// StartCoroutine(AddCircleCollider2D(COROUTINE_DELAY, character));
					character.AddComponent<Rigidbody>();
					character.AddComponent<Pao_CharacterController>();
					gameManager.DetectGiants();
					gameState = GameState.PLATFORMER;
				}  
				break;
			case GameState.PLATFORMER:
				Cursor.visible = false;
				Debug.Log("You've collected " + score + " giants!");
				break;
			default:

				break;
		}

	}

	IEnumerator AddCircleCollider2D(float delay,GameObject character_){
		yield return new WaitForSeconds(delay);
		character_.AddComponent<BoxCollider2D>();
	}

	
}
