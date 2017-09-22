using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerScript_dmf463 : InputManagerScript {

    public override void Start()
    {
        base.Start();
    }

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
                    Debug.Log("gridPos of selected = " + gameManager.GetPositionOfTokenInGrid(selected));
                }
                else
                {
                    //if we already have a selection, then get the grid positions of the two objects
                    //(selected and collider)

                    //position of first object selected
                    Vector2 pos1 = gameManager.GetPositionOfTokenInGrid(selected);
                    Debug.Log("pos1 = " + pos1);

                    //position of second object selected
                    Vector2 pos2 = gameManager.GetPositionOfTokenInGrid(collider.gameObject);
                    Debug.Log("pos2 = " + pos2);
                    Debug.Log("absolute value = " + Mathf.Abs((pos1.x - pos2.x) + (pos1.y - pos2.y)));

                    //check if these two are length 1 away, then evaluate Token exchange
                    //the original code had it finding the absolute value for the entire thing
                    //which threw off the order of operations, causing bad math
                    if (Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y) == 1)
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
