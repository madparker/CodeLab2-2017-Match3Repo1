using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pao_CharacterController : MonoBehaviour {

	Rigidbody rb;
	public float speed = 10f;
	public float jumpSpeed = 9.8f;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
 	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(Input.GetKey(KeyCode.D)){
			// rb.velocity = Vector3.right * speed * Time.deltaTime;
			transform.position += Vector3.right * speed * Time.deltaTime;	
		}		
		if(Input.GetKey(KeyCode.S)){
			// rb.velocity = Vector3.down * speed * Time.deltaTime;
			transform.position += Vector3.down * speed * Time.deltaTime;	
		}
		if(Input.GetKey(KeyCode.A)){
			// rb.velocity = Vector3.left * speed * Time.deltaTime;
			transform.position += Vector3.left * speed * Time.deltaTime;	
		}
		if(Input.GetKey(KeyCode.W)){
			// rb.velocity = Vector3.up * speed * Time.deltaTime;
			// rb.AddForce(Vector3.up * jumpSpeed);
			transform.position += Vector3.up * jumpSpeed * Time.deltaTime;	
		}
	}
	


}
