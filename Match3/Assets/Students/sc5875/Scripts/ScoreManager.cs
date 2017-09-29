using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
	[SerializeField] int scores;
	public int Scores{get{return scores;}}
	// Use this for initialization
	void Start () {
		scores = 0;
	}
	public void SetScore(int m_score){
		scores += m_score;
		if(m_score >= 4)
		scores += 5;

	}
}
