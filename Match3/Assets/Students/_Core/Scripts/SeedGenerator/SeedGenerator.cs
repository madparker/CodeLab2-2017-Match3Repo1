using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGenerator : MonoBehaviour {

	public int initSeed;

	void Awake(){
		Random.InitState(initSeed);
	}
}
