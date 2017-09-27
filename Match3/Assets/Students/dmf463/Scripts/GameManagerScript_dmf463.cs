using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript_dmf463 : GameManagerScript
{
    protected MatchManagerScript_dmf463 dm_matchManager;

    // Use this for initialization
    public override void Start()
    {
        tokenTypes = (Object[])Resources.LoadAll("_Core/Tokens/"); //grabbing prefabs
        gridArray = new GameObject[gridWidth, gridHeight]; //creating the grid
        matchManager = GetComponent<MatchManagerScript>(); //assigning scripts to variables
        dm_matchManager = GetComponent<MatchManagerScript_dmf463>();
        inputManager = GetComponent<InputManagerScript>();
        repopulateManager = GetComponent<RepopulateScript>();
        moveTokenManager = GetComponent<MoveTokensScript>();

        //our new way to ensure that the grid doesn't start with matches
        MakeGridNew();
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

    void MakeGridNew()
    {
        grid = new GameObject("TokenGrid");
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                AddTokenToPosInGridNew(x, y, grid);
            }
        }
    }

    //adds random token types to specific positions in the grid
    public void AddTokenToPosInGridNew(int x, int y, GameObject parent)
    {
        Vector3 position = GetWorldPositionFromGridPosition(x, y);

        int rnd = Random.Range(0, tokenTypes.Length);

        //find a random token type and add it
        GameObject token = Instantiate(tokenTypes[rnd], position, Quaternion.identity) as GameObject;
        token.transform.parent = parent.transform;
        gridArray[x, y] = token;

        //if there is a horizontal match, change one of the tokens
        if (x > 1 && dm_matchManager.GridHasHorizontalMatch(x - 2, y))
        {
            rnd = (rnd + 1) % tokenTypes.Length;
            token.GetComponent<SpriteRenderer>().sprite = (tokenTypes[rnd] as GameObject).GetComponent<SpriteRenderer>().sprite;

        }

        //do the same for vertical matches
        if (y > 1 && dm_matchManager.GridHasVerticalMatch(x, y - 2))
        {
            rnd = (rnd + 1) % tokenTypes.Length;
            token.GetComponent<SpriteRenderer>().sprite = (tokenTypes[rnd] as GameObject).GetComponent<SpriteRenderer>().sprite;
        }

    }
}
