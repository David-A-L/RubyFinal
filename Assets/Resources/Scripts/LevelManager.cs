using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class LevelManager : MonoBehaviour {

	//all balls in level, updated and maintained solely by level manager
	//
	//public ArrayList<>
	LevelPhysicsDriver physicsDriver;

	// Use this for initialization
	void Start () {
		physicsDriver = new LevelPhysicsDriver (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
