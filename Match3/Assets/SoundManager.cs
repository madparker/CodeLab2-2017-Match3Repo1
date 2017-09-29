using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	//Audio
	public AudioSource source1;
	public AudioSource source2;
	public AudioClip match;
	public AudioClip noMatch;
	public AudioClip scoreIncrease;
	public AudioClip tokenFillSpaceSound;
	public float pitchMin;
	public float pitchMax; 

	public void RandomizeVolandPitch (AudioSource _source) {
		_source.pitch = Random.Range(0.94f, 1.6f);
		_source.volume = Random.Range(0.8f, 1f);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
