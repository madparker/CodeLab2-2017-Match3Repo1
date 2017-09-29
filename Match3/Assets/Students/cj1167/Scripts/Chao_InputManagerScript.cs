using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chao_InputManagerScript : MonoBehaviour {

	protected Chao_GameManagerScript gameManager;
	protected Chao_MoveTokensScript moveManager;
	protected GameObject selected = null;

	public virtual void Start () {
		moveManager = GetComponent<Chao_MoveTokensScript>();
		gameManager = GetComponent<Chao_GameManagerScript>();
	}

	public void SelectToken ()
	{
		//Use mouse as game controller
		if(Input.GetMouseButtonDown(0)){
			//Get the world point of the mouse
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//Give the unit player clicked a collider - Smart!!
			Collider2D collider = Physics2D.OverlapPoint(mousePos);

			//Question: In which case the collider would be null?
			if(collider != null){

				//if nothing has been selected, give this collider to selected, which means the unit player clicked is now selected.
				if(selected == null){

					selected = collider.gameObject;
					//Give player feedback: hey, you have selected this. Audio, animation and color changes. Right now color is the easiest solution.
					selected.gameObject.GetComponent<SpriteRenderer> ().color = Color.gray;

				} 
				//if the player has selected an unit, then second click will ask game manager to do a exchange.
				else {

					//pos1 is the position of the unit which has been selected
					Vector2 pos1 = gameManager.GetPositionOfTokenInGrid(selected);
					//pos2 is the positon of the unit player wants to do a exchange
					Vector2 pos2 = gameManager.GetPositionOfTokenInGrid(collider.gameObject);
					//Only if this two are next to each other, then they can do a exchange
					if(Mathf.Abs((pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y)) == 1){

						moveManager.SetupTokenExchange(selected, pos1, collider.gameObject, pos2, true);
						//change the color back after each exchange.
						selected.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
					}
					//after exchange, nothing has been selected, ready to do a new exchange.
					selected = null;
				}
			}
		}
	}

}
