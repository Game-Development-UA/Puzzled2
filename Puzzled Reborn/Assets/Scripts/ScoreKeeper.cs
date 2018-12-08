using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour {
	[Range(0.1f, 10f)][SerializeField] float speed = 1f;
	[SerializeField] int scorePerBlock = 50;
	[SerializeField] TextMeshProUGUI scoreText;
	[SerializeField] int score = 0;



	// Use this for initialization
	void Start () {
		scoreText.text = score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		Time.timeScale = speed;
	}

	public void AddToScore(){
		score += scorePerBlock;
		scoreText.text = score.ToString();
	}
}
