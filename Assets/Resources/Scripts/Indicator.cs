using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Indicator : MonoBehaviour {

	private LevelManager levelManager;
	private Image rend;
	// Use this for initialization
	void Start () {
		levelManager = GameObject.Find ("Main Camera").GetComponent<LevelManager> ();
		rend = transform.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rend.color = levelManager.getCurrentPlayer ().material.color;
		//transform.Rotate (Vector3.up, Time.deltaTime * 45);
	}
}
