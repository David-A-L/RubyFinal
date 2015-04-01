using UnityEngine;
using System.Collections;



public class FinishScript : MonoBehaviour {
	private LevelManager levelManager;
	public Material myMaterial;
	public int marbleCount = 0; // Number of marbles to hit the platform before it disappears
	//TODO MOVE FINISH/LIFE CONTROL TO THE LEVEL MANAGER
	// Use this for initialization
	void Start () {
		levelManager = GameObject.Find ("Main Camera").GetComponent<LevelManager> ();
	}

	void Update() {
		Object[] marbles = GameObject.FindGameObjectsWithTag ("marble");

		//If no marbles left restart level.
		if (marbles.Length == 0) {
			levelManager.endLevel ();
			//Destroy(gameObject);
		}
	}


	void OnCollisionEnter(Collision coll){
		if (coll.collider.gameObject.tag == "marble") {

			//Add points to player's score
			levelManager.updatePlayerScore(coll.collider.gameObject);

			//Remove traces of marble
			levelManager.ballList.Remove(coll.collider.gameObject);
			Destroy (coll.collider.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
