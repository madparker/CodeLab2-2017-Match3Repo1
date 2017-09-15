using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MattMatchManager : MatchManagerScript {

	public override bool GridHasMatch ()
	{
		bool hasMatch = base.GridHasMatch ();

		// I PUT SOME CODE IN HERE TO CHECK FOR VERTICAL MATCHES
		// hasMatch = WHATEVER THIS SHOULD BE || hasMatch;

		return hasMatch;
	}
}
