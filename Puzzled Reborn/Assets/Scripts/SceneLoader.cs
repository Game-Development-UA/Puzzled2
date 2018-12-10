using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public void LoadMenu() {
		SceneManager.LoadScene(0);
	}

	public void LoadLevel() {
		SceneManager.LoadScene(1);
	}

	public void ExitPuzzled() {
		Application.Quit();
	}
}