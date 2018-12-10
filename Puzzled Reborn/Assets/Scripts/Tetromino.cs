using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {

	public Transform[] cubes;
	public float reach = 100f;

	Color[] originalColors = new Color[4];
	Color highlightColor, activeColor, defaultColor;
	RaycastHit hit;


	public void SetHighlightColor(Color color) {
		highlightColor = color;
	}

	public void SetDefaultColor(Color color) {
		defaultColor = color;
	}

	public void SetActiveColor(Color color) {
		activeColor = color;
		for(int i=0; i < 4; i++) { 
			cubes[i].gameObject.GetComponent<Renderer>().material.color = color;
		}
	}

	public void SetDefault() {
		for (int i=0; i < 4; i++) {
			// Get position of current cubes bottom face.
			Vector3 pos = new Vector3(
				cubes[i].position.x,
				cubes[i].position.y - 0.5f,
				cubes[i].position.z
				);

			// Change material to the default color.
			if(Physics.Raycast(pos, Vector3.down, out hit, reach)) {
				GameObject target = hit.collider.gameObject;
				target.GetComponent<Renderer>().material.color = defaultColor;
			}
		}
	}

	public void SetHighlight() {
		// Highlight projected drop location.
		for(int i=0; i < 4; i++) {
			// Get position of current cubes bottom face.
			Vector3 pos = new Vector3(
					cubes[i].position.x,
					cubes[i].position.y - 0.5f,
					cubes[i].position.z
					);

			if(Physics.Raycast(pos, Vector3.down, out hit, reach)) {
				GameObject target = hit.collider.gameObject;
				Renderer renderer = target.GetComponent<Renderer>();
				if (renderer.material.color != activeColor) {
					originalColors[i] = renderer.material.color;
					renderer.material.color = highlightColor;
				}
			}
		}
	}

	public void MoveToNewPosition(Vector3[] newPositions) {
		// Undo highlight
		SetDefault();
		// Assumed that the move is valid
		for(int i=0; i < 4; i++) {
			// Move shape to a new position
			cubes[i].position = newPositions[i];
		}
		SetHighlight();
		SetActiveColor(activeColor);
	}

}
