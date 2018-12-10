using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	// Store color for active tetromino.
	public Color activeColor;
	// Store color for the projected landing location.
	public Color highlightColor;
	// Store Prefabs for spawning random shapes.
	public GameObject[] tetrominos;

	RaycastHit hit;
	GameObject activePiece;
	Tetromino activeScript;

	// Use this for initialization
	void Start () {
		SpawnTetromino();
	}

	// Update is called once per frame
	void Update () {
		if ( Input.GetMouseButtonDown(0) ) {
			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if ( Physics.Raycast(ray, out hit, 100.0f) ) {
				Debug.Log("Hit!");
				Debug.Log("   Details: " + hit.collider.gameObject.name);
			}
		}
	}

	void SpawnTetromino() {
		// Instantiate one of the six possible tetrominos.
		activePiece = Instantiate(tetrominos[Random.Range(0, 6)]);
		// Set active color.
		activePiece.GetComponent<Renderer>().material.color = activeColor;

		activeScript = activePiece.GetComponent<Tetromino>();
		// Set highlight color for projected destination.
		activeScript.SetHighlightColor(highlightColor);
	}

	void CheckScore() {
		;
	}
}
