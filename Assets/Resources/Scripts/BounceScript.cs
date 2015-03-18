using UnityEngine;
using System.Collections;

public class BounceScript : MonoBehaviour {
	
	private ParticleRenderer rend;
	private bool displayed = false;
	private float timer = 0.35f;
	
	// Use this for initialization
	void Start () {
		rend.enabled = false;
	}
	
	void OnTriggerEnter (Collider coll){
		if (coll.gameObject.tag == "marble") {
			coll.attachedRigidbody.AddForce (Vector3.up * 700f);
			rend = this.gameObject.GetComponent<ParticleRenderer> ();
			rend.enabled = true;
			displayed = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (displayed) {
			timer -= Time.deltaTime;
			if (timer <= 0){
				rend.enabled = false;
				displayed = false;
				timer = 0.35f;
			}
		}
	}
}
