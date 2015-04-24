using UnityEngine;
using System.Collections;

public class simplePortScript : MonoBehaviour {

	public Vector3 dest;
	public bool stopZVelocity;//for adding depth which can't realistically be a part of the game

	// Use this for initialization
	void OnTriggerEnter(Collider coll){
		if(coll.gameObject.tag != "marble"){return;}
		print ("hooray");
		Vector3 temp = coll.transform.position;
		temp = dest;
		coll.gameObject.transform.position = temp;
		if (stopZVelocity) {
			Vector3 vel = coll.attachedRigidbody.velocity; 
			vel.z = 0;
			coll.attachedRigidbody.velocity = vel;
		}
	}
	


//	void OnTriggerEnter(Collider coll){
//		if (coll.tag == "marble") {
//			//TODO let level manager know that I have destroyed a ball so it can do the bookkeeping
//			Jukebox.Instance.playASound("BALL_EXPLOSION");
//			print ("hit hazard");
//			GameObject.Destroy(coll.gameObject);
//			Debug.Log("A Hazard Destroyed a ball");
//		}
//	}
}
