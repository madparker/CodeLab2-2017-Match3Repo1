using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript_dmf463 : GameManagerScript {

	// Use this for initialization
	public override void Start ()
    {
        base.Start();
    }

    public override void Update()
    {
        if (!GridHasEmpty())
        { //if grid is fully populated
            if (matchManager.GridHasMatch())
            {
                //remove the matches
                matchManager.RemoveMatches();
            }
            else inputManager.SelectToken(); //allow token to be selected
        }
        else
        { //if grid not fully populated
            if (!moveTokenManager.move) moveTokenManager.SetupTokenMove(); //set it true so they can fill empty space
            if (!moveTokenManager.MoveTokensToFillEmptySpaces()) repopulateManager.AddNewTokensToRepopulateGrid(); //and there are still free spaces, allow it to populate
        }
    }

    public override bool GridHasEmpty()
    {
        return base.GridHasEmpty();
    }

}
