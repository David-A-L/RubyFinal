using UnityEngine;
using System.Collections;

/// <summary>
/// Generic script that can be attached to any trigger field. Will pull any marble toward the field's transform.position
/// </summary>
public class Suction : MonoBehaviour {
	public float power = 20f;
	
	void OnTriggerStay(Collider other){
		if (other.tag == "marble") {
			Vector3 dir = (transform.position - other.transform.position).normalized;
			other.attachedRigidbody.AddForce(dir * power);
		}
	}
}
