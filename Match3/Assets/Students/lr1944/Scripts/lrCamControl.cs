using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lrCamControl : MonoBehaviour {

	[SerializeField] float speed;
	float inputH;
	Vector3 origin;

	void Start () {
		origin = new Vector3 (0, 0, 0);
	}

	void Update () {
		inputH = Input.GetAxis ("Horizontal");
		transform.RotateAround (origin, Vector3.up, -inputH * speed * Time.deltaTime);
	}
}
