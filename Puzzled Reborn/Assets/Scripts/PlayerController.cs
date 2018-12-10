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
	public float longPressDelay = 0.2f;

	RaycastHit hit;
	GameObject activePiece;
	GameObject currentSelection;
	Tetromino activeScript;

	// Track long click
	float startTime;


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
		activeScript.SetHighlight();

		// If mouse clicked.
		if ( Input.GetButtonDown("Fire1") ) {
			startTime = Time.time;

			// Cast ray from the camera to mouse poisiton.
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			// Check the collision.
			if ( Physics.Raycast(ray, out hit, 100.0f) ) {
				Debug.Log("Hit!");
				Debug.Log("   Details: " + hit.collider.gameObject.name);
				currentSelection = hit.collider.gameObject;

				// Check RotateUI was clicked
			}
			// Store time to differentiate long clicks.
		}
		// If left click longer than longPressDelay.
		else if ( Input.GetButton("Fire1") && Time.time - startTime > longPressDelay ) {
			// Keep original selection position
			Vector3 ogClickPosition = currentSelection.transform.position;
			// Get renderer to check for highlight
			Renderer renderer = currentSelection.GetComponent<Renderer>();
			if (renderer.material.color == highlightColor) {
				// Store new 
				Vector3[] newPositions = new Vector3[4];

				// Cast ray from the camera to mouse poisiton.
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if ( Physics.Raycast(ray, out hit, 100.0f) ) {
					// Get selected area of the playable area
					Vector3 selectedLocation = hit.collider.gameObject.transform.position;

					Vector3 offset = selectedLocation - ogClickPosition;

					// Check cube location
					for( int i = 0; i < 4; i++ ) {
						Vector3 currentPosition = activeScript.cubes[i].position;
						currentPosition += offset;
						if(isValidPosition(currentPosition)) {
							newPositions[i] = currentPosition;
						} else {
							return;
						}
					}

					// If all 4 were valid positions, move to it.
					activeScript.MoveToNewPosition(newPositions);
				}
				
			}
		}
	}

	bool isValidPosition(Vector3 input) {
		GameObject[] pieces = GameObject.FindGameObjectsWithTag("Tetromino");
		for( int i = 0; i < pieces.Length; i++) {
			Vector3 position = pieces[i].transform.position;
			if (
				position == input ||
				position.x > 2.0f || position.z > 2.0f ||
				position.x < -2.0f || position.z < -2.0f
				) {
				return false;
			}
		}
		return true;
	}

	Vector3 roundVector3(Vector3 input) {
		return new Vector3(
			Mathf.Round(input.x),
			Mathf.Round(input.y),
			Mathf.Round(input.z)
		);
	}

	void SpawnTetromino() {
		// TODO: Check if gameOver before Instantiation.

		// Instantiate one of the six possible tetrominos.
		activePiece = Instantiate(tetrominos[Random.Range(0, 6)]);
		activePiece.transform.position = spawnLocation;

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
