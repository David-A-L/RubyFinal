using UnityEngine;
using System.Collections;

public class EnableMarble : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Rigidbody mRigid = this.gameObject.GetComponent<Rigidbody>();
		if (Input.GetKey(KeyCode.A) && mRigid == null) {
			gameObject.AddComponent<Rigidbody> ();
		}
	}
}
