using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGreatInputManagerScriptExtensionByDennis : InputManagerScript {

    public override void SelectToken()
    {
        if (Input.GetMouseButtonDown(0))
        {

            //project mouse position from screen pos to world position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //checks if there's a collider at mousePos
            Collider2D collider = Physics2D.OverlapPoint(mousePos);

            if (collider != null)
            {
                if (selected == null)
                {
                    //select the thing the mouse is pointed at
                    selected = collider.gameObject;
                }

                else
                {
                    //if we already have a selection, then get the grid positions of the two objects
                    //(selected and collider)

                    //position of first object selected
                    Vector2 pos1 = gameManager.GetPositionOfTokenInGrid(selected);

                    //position of second object selected
                    Vector2 pos2 = gameManager.GetPositionOfTokenInGrid(collider.gameObject);

                    //check if these two are length 1 away, then evaluate Token exchange
                    if (Mathf.Abs(pos1.x - pos2.x) <= 1
                        && Mathf.Abs(pos1.y - pos2.y) <= 1)
                    {
                        //setup token exchange will try to swap the two items; if this doesn't make a match, move them back
                        moveManager.SetupTokenExchange(selected, pos1, collider.gameObject, pos2, true);
                    }

                    selected = null;
                }
            }
        }
    }
}
