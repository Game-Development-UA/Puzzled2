using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPauseMenu : MonoBehaviour {

	public GameObject panel;
	public GameObject gameOver;
	public GameObject helpText;

	
	public void togglePause() {
		if(panel.active) {
			unpauseGame();
		}
		else {
			pauseGame();
		}
	}
	public void pauseGame(){
		panel.SetActive(true);
		Time.timeScale = 0f;
	}

	public void unpauseGame(){
		panel.SetActive(false);
		Time.timeScale = 1f;
	}

	public void GameOver() {
		togglePause();
		gameOver.SetActive(true);
		helpText.SetActive(false);
	}
}