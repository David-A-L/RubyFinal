using UnityEngine;
using System.Collections;

public class BlackHole : MonoBehaviour {

	public float Gconstant;
	public float maxForce;

	//Might want to enable this script on level run (so it doesn't pull newly spawned balls)
	void OnTriggerStay(Collider other){
		if (other.tag != "marble")
			return;
		Vector3 dir = this.transform.position - other.transform.position;
		float pow = Gconstant * this.GetComponent<Rigidbody> ().mass * other.attachedRigidbody.mass / (dir.sqrMagnitude);
		if (pow < maxForce)
			other.attachedRigidbody.AddForce (pow * dir.normalized);
	}
}