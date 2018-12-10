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
	// Store location for the new tetrominos
	public Vector3 spawnLocation;

	RaycastHit hit;
	GameObject activePiece;
	Tetromino activeScript;

	// Use this for initialization
	void Start () {
		// Ensure random piece by setting different seed each time.
		Random.seed = (int)System.DateTime.Now.Ticks;

		// Set alpha values for the color
		activeColor.a = 1f;
		highlightColor.a = 1f;

		// Instantiate the first piece
		SpawnTetromino();
	}

	// Update is called once per frame
	void Update () {
		// If mouse clicked.
		if ( Input.GetMouseButtonDown(0) ) {
			// Cast ray from the camera to mouse poisiton.
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			// Check the collision.
			if ( Physics.Raycast(ray, out hit, 100.0f) ) {
				Debug.Log("Hit!");
				Debug.Log("   Details: " + hit.collider.gameObject.name);
			}
		}
	}

	void SpawnTetromino() {
		// Instantiate one of the six possible tetrominos.
		activePiece = Instantiate(tetrominos[Random.Range(0, 6)]);

		// Get Tetromino Object.
		activeScript = activePiece.GetComponent<Tetromino>();

		// Set color of the active piece.
		activeScript.SetActiveColor(activeColor);
		// Set highlight color for projected destination.
		activeScript.SetHighlightColor(highlightColor);
	}

	void CheckScore() {
		;
	}
}
