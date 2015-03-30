using UnityEngine;
using System.Collections;

public class LevelCanvasManager : MonoBehaviour {
	public static LevelCanvasManager instance;
	/*
	 * Script to manage and control ui Elements for this scene
	 * 
	 * 
	 */ 

	void Awake(){
		if (instance) {
			DestroyImmediate(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
