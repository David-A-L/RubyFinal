using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour {

	private LevelManager levelManager;
	private CanvasRenderer rend;
	// Use this for initialization
	void Start () {
		levelManager = GameObject.Find ("Main Camera").GetComponent<LevelManager> ();
		rend = transform.GetComponent<CanvasRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		rend.SetMaterial(levelManager.getCurrentPlayer ().material, null);
		//transform.Rotate (Vector3.up, Time.deltaTime * 45);
	}
}
