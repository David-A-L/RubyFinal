using UnityEngine;
using System.Collections;
using UnityEngine.UI;
	

public class FinishScript : MonoBehaviour {
	private LevelManager levelManager;
	public Material myMaterial;
	public bool shrinkablePlatform = false;
	public int marbleCount = -1; // Number of marbles to hit the platform before it disappears
	// Set in inspector, otherwise default behavior is never to disappear
	private Vector3 platformStartSize;
	static bool levelFinished = false;
	//TODO MOVE FINISH/LIFE CONTROL TO THE LEVEL MANAGER
	// Use this for initialization
	void Start () {
		levelManager = GameObject.Find ("Main Camera").GetComponent<LevelManager> ();
		platformStartSize = transform.localScale;
	}

	void Update() {
		if (!levelFinished) {
			Object[] marbles = GameObject.FindGameObjectsWithTag ("marble");

			if (marbles.Length == 0) {
				marbles = GameObject.FindGameObjectsWithTag ("static_marble");
			}

			//If no marbles left restart level.
			if (marbles.Length == 0) {
				levelManager.showFinishCanvas ();
				levelFinished = true;
				//Destroy(gameObject);
			}
		}
	}
	void shrinkPlatform() {

		Vector3 newScale = transform.localScale;
		if (platformStartSize.x > platformStartSize.y) {
			newScale.x *= 0.9f;
		} else {
			newScale.y *= 0.9f;
		}
		transform.localScale = newScale;
	}

	void OnCollisionEnter(Collision coll){
		if (coll.collider.gameObject.tag == "marble") {

			//Add points to player's score
			levelManager.updatePlayerScore(coll.collider.gameObject);

			//Remove traces of marble
			levelManager.ballList.Remove(coll.collider.gameObject);
			Destroy (coll.collider.gameObject);

			marbleCount--;
			GameObject thing = (GameObject)Instantiate(Resources.Load ("Prefabs/plus"));
			thing.transform.Translate(new Vector3(0,100,0));
			print(thing.transform.position);
			if(shrinkablePlatform) {
				shrinkPlatform();
			}
			if(marbleCount == 0) {
				Destroy(gameObject);
			}
		}
	}
}
