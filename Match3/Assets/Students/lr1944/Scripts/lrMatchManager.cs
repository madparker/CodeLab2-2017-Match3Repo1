using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lrMatchManager : MatchManagerScript {


	public override bool GridHasMatch (){
	
		bool hasMatch =  base.GridHasMatch ();
		Debug.Log (hasMatch);

		for (int x = 0; x < gameManager.gridWidth; x++) {

			for (int y = 0; y < gameManager.gridHeight; y++) {

				if (y < gameManager.gridHeight - 2) {
					
					hasMatch = hasMatch || GridHasVerticalMatch(x, y);
					
				}
			}

		}

		return hasMatch;

	}

	public bool GridHasVerticalMatch (int x, int y) {

		GameObject token1 = gameManager.gridArray[x,y+0];
		GameObject token2 = gameManager.gridArray[x,y+1];
		GameObject token3 = gameManager.gridArray[x,y+2];

		if (token1 != null && token2 != null && token3 != null) {

			SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer> ();
			SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer> ();
			SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer> ();


			return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
		} else {
			return false;
		}

	}

	public int GetVerticalMatchLength (int x, int y) {
		int matchLength = 1;

		GameObject first = gameManager.gridArray [x, y];

		if (first != null) {

			SpriteRenderer sr1 = first.GetComponent<SpriteRenderer> ();

			for (int i = y + 1; i < gameManager.gridHeight; i++) {
				GameObject other = gameManager.gridArray [x, i];

				if (other != null) {
					SpriteRenderer sr2 = other.GetComponent<SpriteRenderer> ();

					if (sr1.sprite == sr2.sprite) {
						matchLength++;
					} else {
						break;
					}
				}
			}
		}
		return matchLength;
	}



	public override int RemoveMatches() {
		int numRemoved = 0;

		for (int x = 0; x < gameManager.gridWidth; x++) {

			for (int y = 0; y < gameManager.gridHeight; y++) {

				if (x < gameManager.gridWidth - 2) {
					int horizonMatchLength = GetHorizontalMatchLength (x, y);

					if (horizonMatchLength > 2) {

						for (int i = x; i < x + horizonMatchLength; i++) {
							GameObject token = gameManager.gridArray [i, y];
							Destroy (token);

							gameManager.gridArray [i, y] = null;

							numRemoved++;
						}
					}
				}

				if (y < gameManager.gridHeight - 2) {
					int vertMatchLength = GetVerticalMatchLength (x, y);

					if (vertMatchLength > 2) {

						for (int i = y; i < y + vertMatchLength; i++) {
							GameObject token = gameManager.gridArray [x, i];
							Destroy (token);

							gameManager.gridArray [x, i] = null;

							numRemoved++;
						}
					}
				}
				 

			}
		}
//		Debug.Log (numRemoved);

		return numRemoved;


	}
}

