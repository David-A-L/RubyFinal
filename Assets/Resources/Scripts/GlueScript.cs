using UnityEngine;
using System.Collections;

public class GlueScript : MonoBehaviour {
	public static float stickStrength = 7f;
	public static float minSpeed = 2f;
	// If a marble rolls through a glue patch, speed will be lowered
	void OnTriggerStay(Collider other){
		if (other.tag == "marble") {
			if (other.attachedRigidbody.velocity.magnitude > minSpeed){
				other.attachedRigidbody.AddForce(
					-other.attachedRigidbody.velocity * stickStrength);
			}
		}
	}
	
}
