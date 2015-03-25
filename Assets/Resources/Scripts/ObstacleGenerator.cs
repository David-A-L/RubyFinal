using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleGenerator : MonoBehaviour {
	
	public float minXrange;
	public float maxXrange;
	public float minYrange;
	public float maxYrange;
	public float minDist;
	public int numGenericGenerate;
	public int numBlackHoles;
	
	public GameObject safeZone;
	
	List<Vector3> obstaclePos;
	
	// Use this for initialization
	void Start () {
		obstaclePos = GameManager.Instance.levelRepeat();
		if(obstaclePos == null) {
			obstaclePos = new List<Vector3>();
			Random.seed = (int)System.DateTime.Now.Ticks;
		}
		
		if(numGenericGenerate > 0) {
			numGenericGenerate += 2*GameManager.Instance.completedLevels;
		}
		if(numBlackHoles > 0) {
			numBlackHoles += 2*GameManager.Instance.completedLevels;
		}
		
		for(int i = 0; i < numGenericGenerate; ++i) {
			genObstacle("obstacle");
		}
		for(int i = 0; i < numBlackHoles; ++i) {
			genObstacle("blackHole");
		}
		
	}
	
	void genObstacle(string objType){
		
		GameObject newObst = null;
		Vector3 potLoc;
		do {
			potLoc = new Vector3 (Random.Range (minXrange, maxXrange), Random.Range (minYrange, maxYrange), 0f);
		} while (!validLocation(potLoc));
		
		switch(objType) {
		case "obstacle":
			newObst = ((GameObject)Instantiate (Resources.Load ("Prefabs/Obstacle")));
			break;
		case "blackHole":
			newObst = ((GameObject)Instantiate (Resources.Load ("Prefabs/bHole")));
			break;
		default:
			print ("Invalid obstacle type");
			break;
		}
		if(newObst != null) {
			newObst.transform.position = potLoc;
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
			Application.LoadLevel (Application.loadedLevel);
		}
	}
}