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
	float startTime, dropTime;


	// Use this for initialization
	void Start () {
		// Ensure random piece by setting different seed each time.
		Random.seed = (int)System.DateTime.Now.Ticks;

		// Set alpha values for the color
		activeColor.a = 1f;
		highlightColor.a = 1f;
		defaultColor.a = 1f;

		dropTime = Time.time;
		// Instantiate the first piece
		SpawnTetromino();
	}

	// Update is called once per frame
	void Update () {
		activeScript.SetHighlight();
		if ( Time.time - dropTime > 1 || Input.GetKeyDown("space")) {
			dropTime = Time.time;

			// Decrease by one
			Vector3[] locs = new Vector3[4];
			for(int i = 0; i < 4; i++ ){
				locs[i] = activeScript.cubes[i].position - Vector3.up;
			}

			if(ValidPositions(locs)) {
				activeScript.MoveToNewPosition(locs);
			} else {
				activeScript.SetDefault();
				activeScript.SetActiveColor(defaultColor);
				SpawnTetromino();
			}
		}
		// If mouse clicked.
		if ( Input.GetButtonDown("Fire1") ) {
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
		// If left click longer than longPressDelay.
		if ( Input.GetButton("Fire1") && Time.time - startTime > longPressDelay ) {
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
		
	}

	Vector3 roundVector3(Vector3 input) {
		return new Vector3(
			Mathf.Round(input.x),
			Mathf.Round(input.y),
			Mathf.Round(input.z)
		);
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
	
	bool insideBorder(Vector3 pos) {
		return(
			pos.x >= -2 && pos.x <= 2 &&
			pos.z >= -2 && pos.z <= 2 &&
			pos.y >= 0);
	}

	bool ValidPositions(Transform[] positions) {
		for(int i = 0; i < positions.Length; i++) {
			if (!insideBorder(positions[i].position)) {
				return false;
			}

			// Check if there is not a cube in the way.
			CheckDestination(positions[i].position);
		}
		return true;
	}

	bool ValidPositions(Vector3[] positions) {
		for(int i = 0; i < positions.Length; i++) {
			if (!insideBorder(positions[i])) {
				return false;
			}

			// Check if there is not a cube in the way.
			if(!CheckDestination(positions[i])) {
				return false;
			}
		}
		return true;
	}

	bool CheckDestination(Vector3 target) {
		// Check to ensure no cubes are in the destination position.
		GameObject[] objects = GameObject.FindGameObjectsWithTag("Tetromino");
		for(int i = 0; i < objects.Length; i++) {
			if(objects[i].GetComponent<Renderer>().material.color != activeColor){
				if(objects[i].transform.position == target) {
					return false;
				}
			}
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
		for(int y = 0; y < 11; y++) {
			for(int x = -2; x < 3; x++) {
				for(int z = -2; z < 3; z++) {
					
				}
			}
		}
	}
}
