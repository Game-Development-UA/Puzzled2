using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPauseMenu : MonoBehaviour {

	public GameObject panel; 
	//public SumPause status;

	void Update(){
		Debug.Log(SumPause.status);
		if(SumPause.status == true){
			panel.SetActive(true);
			Time.timeScale = 0f;
		} else{
			panel.SetActive(false);
			Time.timeScale = 1f;
		}
	}

	// public void pauseGame(){
	// 	panel.SetActive(true);
	// 	Time.timeScale = 0f;

	// }

	// public void unpauseGame(){
	// 	panel.SetActive(false);
	// 	Time.timeScale = 1f;
	// }
}