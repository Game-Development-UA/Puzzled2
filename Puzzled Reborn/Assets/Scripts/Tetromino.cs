using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {

	public Transform[] cubes;
	public float reach = 100f;
	public Color activeColor;
	RaycastHit hit;

	// Update is called once per frame
	void Update () {
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
				target.GetComponent<Renderer>().material.color = activeColor;
			}
		}
	}

	public void CleanUp() {
		;
	}
}
