using UnityEngine;
using System.Collections;

public class BounceScript : MonoBehaviour {
	
	private ParticleRenderer rend;
	private bool particlesDisplayed = false;
	private bool blink = true;
	private float onBouncetimer = 0.35f;
	
	private float rotTimer = 1.5f;
	private float rotAngle = 90f;
	private float rotPeriod = 1.5f;
	private float totalRotY = 0f;
	
	// Use this for initialization
	void Start () {
		rend = transform.gameObject.GetComponent<ParticleRenderer> ();
		rend.enabled = false;
	}
	
	void OnTriggerEnter (Collider coll){
		if (coll.gameObject.tag == "marble") {
			//TODO: FIX? 
			//(SHOULD THIS ADD ONLY UPWARD FORCE?
			//"transform.up" FORCE?
			//BALL-RELATIVE UP?)
			coll.attachedRigidbody.AddForce (Vector3.up * 700f);
//			rend = this.gameObject.GetComponent<ParticleRenderer> ();
			rend.enabled = true;
			particlesDisplayed = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!rend) {rend = this.gameObject.GetComponent<ParticleRenderer> ();}
		rotTimer -= Time.deltaTime;

		if (particlesDisplayed) {
			transform.Rotate (Vector3.up, 10f);
			totalRotY += 10f;
			onBouncetimer -= Time.deltaTime;
			if (onBouncetimer <= 0){
				rend.enabled = false;
				particlesDisplayed = false;
				onBouncetimer = 1f;

				float rotOffset = rotAngle - totalRotY;
				transform.Rotate (Vector3.up, rotOffset);
				totalRotY = 0;
				rotTimer = 3f;
				blink = false;

			}
			return;
		}

		if (rotTimer <= 0 && !particlesDisplayed) {
			if (blink){
				float rotOffset = rotAngle - totalRotY;
				transform.Rotate (Vector3.up, rotOffset);
				totalRotY = 0;
			}
			blink = !blink;
			rend.enabled = blink;
			rotTimer = 1.5f;
		}

		if (blink && !particlesDisplayed){
			float rotThisFrame = Time.deltaTime * rotAngle / rotPeriod;
			totalRotY += rotThisFrame;
			transform.Rotate (Vector3.up, rotThisFrame);
		}
	}
}
