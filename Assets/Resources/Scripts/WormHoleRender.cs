using UnityEngine;
using System.Collections;

public class WormHoleRender : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<LineRenderer> ().enabled = true;
		gameObject.GetComponent<LineRenderer> ().SetVertexCount (2);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 start = transform.GetChild (0).position;
		Vector3 end = transform.GetChild (1).position;
		gameObject.GetComponent<LineRenderer> ().SetPosition (0, start);
		gameObject.GetComponent<LineRenderer> ().SetPosition (1, end);
	}
}
