using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lrRotation : MonoBehaviour {

	float speed = 40;


	void Update () {
		transform.RotateAround (gameObject.transform.position, Vector3.up, speed * Time.deltaTime);
	}
}
