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
	Tetromino activePiece;

	// Use this for initialization
	void Start () {
		activePiece = Spawn();
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

	GameObject Spawn() {
		
	}

	void CheckScore() {
		
	}
}
