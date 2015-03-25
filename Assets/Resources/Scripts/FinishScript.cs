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
		GameObject[] marbles = GameObject.FindGameObjectsWithTag("marble");
		foreach (GameObject marble in marbles) {
			if (marble.GetComponent<Renderer> ().material.color == GetComponent<Renderer> ().material.color) {
				marbleCount++;
			}
		}
		if (marbleCount == 0) {
			marbleCount = 1;
		}
	}

	void OnCollisionEnter(Collision coll){
		if (coll.collider.gameObject.tag == "marble") {
			print ("Collision");
			if(coll.collider.GetComponent<Renderer>().material.color == GetComponent<Renderer>().material.color) {
				Object[] marbles = GameObject.FindGameObjectsWithTag("marble");
				marbleCount--;
				if(marbleCount == 0) Destroy(gameObject);
				Destroy (coll.collider.gameObject);
				if(marbles.Length <= 1) {
					print ("Done");
					levelManager.endLevel();
				}
				else {
					print("Not done yet");
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
