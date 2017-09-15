using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailySeedGenerator : SeedGenerator {

	void Awake(){
		int seed = System.DateTime.Now.Year * 1000 + System.DateTime.Now.DayOfYear;

		Random.InitState(seed);
	}
}
