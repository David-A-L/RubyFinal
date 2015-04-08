using UnityEngine;
using System.Collections;

/// <summary>
/// This script will kill any ball that enters the trigger zone of the attached object
/// </summary>
public class HazardScript : MonoBehaviour {
	
	void OnTriggerEnter(Collider coll){
		if (coll.tag == "marble") {
			//TODO let level manager know that I have destroyed a ball so it can do the bookkeeping
			GameObject.Destroy(coll.gameObject);
			Debug.Log("A Hazard Destroyed a ball");
		}
	}
	
	
}
