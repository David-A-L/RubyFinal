using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour {

	private LevelManager levelManager;
	private Renderer rend;
	// Use this for initialization
	void Start () {
		levelManager = GameObject.Find ("Main Camera").GetComponent<LevelManager> ();
		rend = gameObject.GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		rend.material = levelManager.getCurrentPlayer ().material;
		transform.Rotate (Vector3.up, Time.deltaTime * 45);
	}
}
