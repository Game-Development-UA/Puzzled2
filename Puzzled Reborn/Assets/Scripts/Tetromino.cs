using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {

	public Transform[] cubes;
	public float reach = 100f;

	Color[] originalColors = new Color[4];
	Color highlightColor;
	RaycastHit hit;

	public void SetHighlightColor(Color color) {
		highlightColor = color;
	}

	public void SetActiveColor(Color color) {
		for(int i=0; i < 4; i++) { 
			Debug.Log(cubes[i].gameObject.GetComponent<Renderer>());
			cubes[i].gameObject.GetComponent<Renderer>().material.color = color;
		}
	}

	public void SetHiglight() {
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
				originalColors[i] = target.GetComponent<Renderer>().material.color;
				target.GetComponent<Renderer>().material.color = highlightColor;
			}
		}
	}

	public void MoveToNewPosition(Vector3[] newPositions) {
		// Get highlight location from the SetHighlight function.
		for(int i=0; i < 4; i++) {
			// Get position of current cubes bottom face.
			Vector3 pos = new Vector3(
				cubes[i].position.x,
				cubes[i].position.y - 0.5f,
				cubes[i].position.z
				);

			// Change material to it's original color.
			if(Physics.Raycast(pos, Vector3.down, out hit, reach)) {
				GameObject target = hit.collider.gameObject;
				target.GetComponent<Renderer>().material.color = originalColors[i];
			}

			// Move shape to a new position
			cubes[i].position = newPositions[i];
		}
	}

}
