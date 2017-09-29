using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pao_GiantsCollectible : MonoBehaviour {

	Collider coll;
	private GameObject giantsToken;
	// Use this for initialization
	void Start () {
		coll = GetComponent<Collider>();
		coll.isTrigger = true;
	}
	
	// Update is called once per frame

	void OnTriggerEnter(){
		Pao_ScoreManager.score += 1;
		giantsToken.GetComponent<SpriteRenderer>().enabled = false;
		Destroy(gameObject);
 	}

	public void GetGiants(GameObject go){
		giantsToken = go;
	}
}
