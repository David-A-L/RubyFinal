using UnityEngine;
using System.Collections;

public class WHHelper : MonoBehaviour {

	//sends collision to parent to be handled
	void OnTriggerEnter(Collider other){
		transform.parent.GetComponent<WHScript> ().EnterHandler (transform.name, other);
	}
}
