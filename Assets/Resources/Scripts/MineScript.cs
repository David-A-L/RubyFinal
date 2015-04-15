using UnityEngine;
using System.Collections;

public class MineScript : MonoBehaviour {

	float explodeRadius;
	public float power = 1000f;
	int flip = 0;

	public GameObject effectZone;

	// Use this for initialization
	void Start () {
		effectZone = transform.FindChild("EffectZone").gameObject;
		explodeRadius = effectZone.transform.localScale.x;
	}

	public void Update(){
		if (flip == 0){
				effectZone.transform.Rotate (Vector3.forward, 180f * Time.deltaTime);
				effectZone.transform.Rotate (Vector3.up, -90f * Time.deltaTime);
		}
		else if (flip == 1) {effectZone.transform.Rotate (Vector3.forward, -60f * Time.deltaTime);}
		else if (flip == 2){effectZone.transform.Rotate (Vector3.right, 90f * Time.deltaTime);}
		else{effectZone.transform.Rotate (Vector3.up, 30f * Time.deltaTime);}
		if (Time.frameCount % 15 == 0){flip = ++flip % 3;}
	}

	//pretty much stole this from unity api
	public void Detonate(){
		print ("MINE EXPLODED!");
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, 3.8f);
		print (explosionPos);
		print (explodeRadius);
		foreach (Collider hit in colliders) {
			if (hit && hit.GetComponent<Rigidbody>() && hit.tag == "marble"){
				//We might be over-thinking this...
				float ex = hit.attachedRigidbody.velocity.x;
				Vector3 vel = Vector3.zero;
				hit.attachedRigidbody.velocity = vel;
				hit.attachedRigidbody.AddExplosionForce(power, explosionPos, explodeRadius, 0f);
				vel = hit.attachedRigidbody.velocity;
				vel.x += ex;
				hit.attachedRigidbody.velocity = vel;
				
				Jukebox.Instance.playASound ("EXPLOSION");
				print ("added force to marble");
			}
		}
		GameObject.Destroy (gameObject);
	}
}
