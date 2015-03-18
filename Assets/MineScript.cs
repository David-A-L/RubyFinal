using UnityEngine;
using System.Collections;

public class MineScript : MonoBehaviour {

	float explodeRadius;
	public float power = 1000f;
	// Use this for initialization
	void Start () {
		explodeRadius = transform.FindChild("EffectZone").localScale.x;
	}

	//pretty much stole this from unity api
	public void Detonate(){
		print ("MINE EXPLODED!");
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, explodeRadius);
		foreach (Collider hit in colliders) {
			if (hit && hit.GetComponent<Rigidbody>() && hit.tag == "marble"){
				float ex = hit.attachedRigidbody.velocity.x;
				Vector3 vel = Vector3.zero;
				hit.attachedRigidbody.velocity = vel;
				hit.attachedRigidbody.AddExplosionForce(power, explosionPos, explodeRadius, 0f);
				vel = hit.attachedRigidbody.velocity;
				vel.x += ex;
				hit.attachedRigidbody.velocity = vel;
				print ("added force to marble");
			}
		}
		GameObject.Destroy (gameObject);
	}
}
