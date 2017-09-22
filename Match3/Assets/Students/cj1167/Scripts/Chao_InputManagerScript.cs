using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chao_InputManagerScript : InputManagerScript {

	public override void SelectToken ()
	{
		if(Input.GetMouseButtonDown(0)){

			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			Collider2D collider = Physics2D.OverlapPoint(mousePos);

			if(collider != null){
				if(selected == null){

					selected = collider.gameObject;
				} else {

					Vector2 pos1 = gameManager.GetPositionOfTokenInGrid(selected);

					Vector2 pos2 = gameManager.GetPositionOfTokenInGrid(collider.gameObject);

					if(Mathf.Abs((pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y)) == 1){

						moveManager.SetupTokenExchange(selected, pos1, collider.gameObject, pos2, true);
					}

					selected = null;
				}
			}
		}
	}

}
