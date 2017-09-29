using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScores : MonoBehaviour {
	private Text UIscore;
	[SerializeField] ScoreManager scoreManager;
	// Use this for initialization
	void Start () {
		UIscore = GetComponent<Text>();
		UIscore.text = scoreManager.Scores.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		UIscore.text = scoreManager.Scores.ToString();
	}
}
