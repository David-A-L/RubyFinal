using UnityEngine;
using System.Collections;

public class FinishScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.tag == "marble") {
			GameManager.Instance.finishLevel();
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
