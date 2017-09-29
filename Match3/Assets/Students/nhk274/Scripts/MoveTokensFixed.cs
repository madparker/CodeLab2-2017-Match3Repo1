using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTokensFixed : MoveTokensScript {

	// Access Variables
	protected SoundManager SM; 

	/*public AudioSource[3] sourceArray;
	public AudioSource source1;
	public AudioSource source2;
	public AudioSource source3;
	public AudioSource source4;
	*/

	public override void Start ()
	{
		base.Start ();
		SM = GetComponent<SoundManager>();
	
	}

	public override bool MoveTokensToFillEmptySpaces(){
		bool movedToken = false;

		for(int x = 0; x < gameManager.gridWidth; x++){
			for(int y = 1; y < gameManager.gridHeight ; y++){
				if(gameManager.gridArray[x, y - 1] == null){
					for(int pos = y; pos < gameManager.gridHeight; pos++){
						GameObject token = gameManager.gridArray[x, pos];
						if(token != null){
                            MoveTokenToEmptyPos(x, pos, x, pos - 1, token);
                            movedToken = true;
						}
					}
					break; //we break the loop so that the token doesn't move twice. Hey, we hit a null and stop checking. Move it down. Next frame, if there's another null, we'll move it again.
				}
			}
		}

        // If the tokens have reached their final position, stop moving them.
		if(lerpPercent == 1){
			move = false;
		}

		return movedToken;
	} 


	//simply adding audio functionality
	public override void MoveTokenToEmptyPos (int startGridX, int startGridY, int endGridX, int endGridY, GameObject token)
	{
		base.MoveTokenToEmptyPos(startGridX, startGridY, endGridX, endGridY, token);

		/* if (move == false) {
			///Set the clip to audio source
			SM.source1.clip = SM.tokenFillSpaceSound;
			SM.source1.pitch = Random.Range(0.75f, 1.1f);
			SM.source1.volume = Random.Range(0.8f, 1f);
			SM.source1.Play(); //Play clip with random pitch and volume
		} */
	}
}

