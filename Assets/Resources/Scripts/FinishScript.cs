using UnityEngine;
using System.Collections;

public class FinishScript : MonoBehaviour {
	public Material myMaterial;
	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.tag == "marble") {
			print ("Collision");
			if(coll.GetComponent<Renderer>().material.color == GetComponent<Renderer>().material.color) {
				Object[] marbles = GameObject.FindGameObjectsWithTag("marble");
				Destroy(gameObject);
				Destroy (coll.gameObject);
				if(marbles.Length <= 1) {
					print ("Done");
					GameManager.Instance.finishLevel();
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
