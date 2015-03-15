using UnityEngine;
using System.Collections;

public class Suction : MonoBehaviour {
	public float power = 20f;
	
	void OnTriggerStay(Collider other){
		if (other.tag == "marble") {
			Vector3 dir = (transform.position - other.transform.position).normalized;
			other.attachedRigidbody.AddForce(dir * power);
		}
	}
}
