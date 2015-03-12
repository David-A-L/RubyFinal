using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleGenerator : MonoBehaviour {

	public float minXrange;
	public float maxXrange;
	public float minYrange;
	public float maxYrange;
	public float minDist;
	public int numGenerate;
	private int numHoles = 1;
	 
	public GameObject safeZone;

	List<Vector3> obstaclePos;

	// Use this for initialization
	void Start () {
		obstaclePos = GameManager.Instance.levelRepeat();

		if (obstaclePos != null) {
			for (int i = 0; i < obstaclePos.Count; ++i) {
				GameObject cube;
				if (i < numHoles){cube = ((GameObject)Instantiate (Resources.Load ("Prefabs/bHole")));}
				else {cube = ((GameObject)Instantiate (Resources.Load ("Prefabs/Obstacle")));}
				cube.transform.position = obstaclePos[i];
			}
			return;
		}

		numGenerate = numGenerate + (2 * GameManager.Instance.completedLevels);
		obstaclePos = new List<Vector3>();
		Random.seed = (int)System.DateTime.Now.Ticks;
		Vector3 potLoc;

		for (int i = 0; i < (numGenerate + 1); ++i) { //the black hole causes the + 1
			GameObject cube;
			do {potLoc = new Vector3 (Random.Range (minXrange, maxXrange), Random.Range (minYrange, maxYrange), 0f);} 
				while (!validLocation(potLoc));
			if (i < numHoles){cube = ((GameObject)Instantiate (Resources.Load ("Prefabs/bHole")));}
			else {cube = ((GameObject)Instantiate (Resources.Load ("Prefabs/Obstacle")));}
			cube.transform.position = potLoc;
			obstaclePos.Add (potLoc);
		}
	
	}


	bool validLocation(Vector3 potLoc){
		float xval = potLoc.x;
		if (xval > 2.5 && xval < 5.5 && potLoc.y < -1.3) {return false;}
		if (obstaclePos.Count == 0) {return true;}
		foreach (Vector3 existingObst in obstaclePos) {
			if (Vector3.Distance (potLoc, existingObst) < minDist) {return false;}
		}
		return true;
	}
	
	private bool IsInside (Collider test, Vector3 point){
		Vector3 center;
		Vector3 direction;
		Ray ray;
		RaycastHit hitInfo;
		bool hit;
		
		// Use collider bounds to get the center of the collider. May be inaccurate
		// for some colliders (i.e. MeshCollider with a 'plane' mesh)
		center = test.bounds.center;
		
		// Cast a ray from point to center
		direction = center - point;
		ray = new Ray (point, direction);
		hit = test.Raycast (ray, out hitInfo, direction.magnitude);
		
		// If we hit the collider, point is outside. So we return !hit
		return !hit;
	}



	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.S)) {
			GameManager.Instance.resetLevel (obstaclePos);
			Application.LoadLevel (0);
		}
		if (Input.GetKeyUp (KeyCode.K)) {
			if (numHoles > 1) {
				--numHoles;
			}
		}
		if (Input.GetKeyUp (KeyCode.L)) {
			if (numHoles < (numGenerate - 2)) {
				++numHoles;
			}
		}
	}
}
