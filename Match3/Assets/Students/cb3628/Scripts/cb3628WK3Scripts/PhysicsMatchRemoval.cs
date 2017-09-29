using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMatchRemoval : MonoBehaviour, IDestroyable {

    public List<GameObject> matchObjects;


    void Awake() {
        
    }


	// Use this for initialization
	void Start () {
        RemoveObjects();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RemoveObjects() {

        foreach (GameObject sprite in matchObjects) {
            sprite.AddComponent<Rigidbody2D>();
            sprite.GetComponent<Collider2D>().isTrigger = false;


        }

    }


}
