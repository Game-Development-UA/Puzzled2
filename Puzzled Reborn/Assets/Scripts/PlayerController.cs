using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	// Store color for active tetromino.
	public Color activeColor;
	// Store color for the projected landing location.
	public Color highlightColor;
	// Default color same as the tiles.
	public Color defaultColor;
	// Store Prefabs for spawning random shapes.
	public GameObject[] tetrominos;
	// Store location for the new tetrominos
	public Vector3 spawnLocation;
	public float longPressDelay = 0.2f;
	public float swipeTime = 0.4f;

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
		defaultColor.a = 1f;

		// Instantiate the first piece
		SpawnTetromino();
	}

	// Update is called once per frame
	void Update () {
		activeScript.SetHighlight();

		// If left click longer than longPressDelay.
		if ( Input.GetButton("Fire1") && Time.time - startTime > longPressDelay  && Time.time - startTime < swipeTime) {
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
						currentPosition = roundVector3(currentPosition + offset);
						if(insideBorder(currentPosition)) {
							newPositions[i] = currentPosition;
						} else {
							newPositions[i] = new Vector3(10f, 10f, 10f);
						}
					}

					if(ValidPositions(newPositions)) {
						ogClickPosition = roundVector3(ogClickPosition + offset);

						// If all 4 were valid positions, move to it.
						activeScript.MoveToNewPosition(newPositions);
					}
				}
				
			}
		}
		// If mouse clicked.
		else if ( Input.GetButtonDown("Fire1") ) {
			startTime = Time.time;

			// Cast ray from the camera to mouse poisiton.
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			// Check the collision.
			if ( Physics.Raycast(ray, out hit, 100.0f) ) {
				currentSelection = hit.collider.gameObject;

				// RotationUI Logic
				if(currentSelection.GetComponent<Renderer>().material.name == "Default_Prototype (Instance)") {
					startTime += swipeTime;
					// Rotate X
					if (Vector3.right == hit.normal) {
						Debug.Log("Rotate X");
						Rotate(Vector3.right);
					}
					// Rotate Y
					else if (Vector3.up == hit.normal) {
						Debug.Log("Rotate Y");
						Rotate(Vector3.up);
					}
					// Rotate Z
					else if (Vector3.back == hit.normal) {
						Debug.Log("Rotate Z");
						Rotate(Vector3.back);
					}

				}
			}
			// Store time to differentiate long clicks.
		}
	}

	Vector3 roundVector3(Vector3 input) {
		return new Vector3(
			Mathf.Round(input.x),
			Mathf.Round(input.y),
			Mathf.Round(input.z)
		);
	}

	bool insideBorder(Vector3 pos) {
		return(
			pos.x >= -2 && pos.x <= 2 &&
			pos.z >= -2 && pos.z <= 2 &&
			pos.y >= 0);
	}

	void Rotate(Vector3 pos) {
		activeScript.SetDefault();

		Vector3 pivot = activeScript.cubes[0].position;
		for (int i = 0; i < 4; i++) {
			activeScript.cubes[i].RotateAround(pivot, pos, 90);
		}

		if(!ValidPositions(activeScript.cubes)) {
			// Revert Changes
			for (int i = 0; i < 4; i++) {
				activeScript.cubes[i].RotateAround(pivot, pos, -90);
			}
		}

		// Update colors
		activeScript.SetHighlight();
		activeScript.SetActiveColor(activeColor);
	}

	bool ValidPositions(Transform[] positions) {
		for(int i = 0; i < positions.Length; i++) {
			if (!insideBorder(positions[i].position)) {
				return false;
			}

			// TODO: Check if there is not a cube in the way.
		}
		return true;
	}

	bool ValidPositions(Vector3[] positions) {
		for(int i = 0; i < positions.Length; i++) {
			if (!insideBorder(positions[i])) {
				return false;
			}

			// TODO: Check if there is not a cube in the way.
		}
		return true;
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
		// Set default color of shapes
		activeScript.SetDefaultColor(defaultColor);
	}

	void CheckScore() {
		;
	}
}
