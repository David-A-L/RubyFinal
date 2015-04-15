using UnityEngine;
using System.Collections;

public class ConveyorPlatform : MonoBehaviour {

	public float slideVel;
	public float exitVelFraction;
	private bool facingRight;
	private ParticleRenderer rend;
	private Vector3 startPoint;
	private LevelManager levelManager;
	private Color userSpecificParticleColor;

	public static float power = 30f;
	public static float maxSpeed = 100f;

	void Start(){
		Player curPlayer = GameObject.Find ("Main Camera").GetComponent<LevelManager> ().getCurrentPlayer ();
		transform.gameObject.layer = LayerMask.NameToLayer (curPlayer.getPhysicsLayerName());
		rend = GetComponentInChildren<ParticleRenderer> ();
		userSpecificParticleColor = curPlayer.material.color;
		rend.material.SetColor("_TintColor", userSpecificParticleColor);
		startPoint = transform.position;
	}

//	public void SetParticleMaterial(Material m){
//		rend.materials[0] = m;
//	}

	void Update(){
		/*
		Vector3 difference = transform.position - startPoint;
		Vector3 Vel = GetComponentInChildren<EllipsoidParticleEmitter>().worldVelocity;
		bool higherNow = transform.position.y > startPoint.y;
		Vel = higherNow ? difference : -difference;
		Vel = Vel.normalized;
		GetComponentInChildren<EllipsoidParticleEmitter>().worldVelocity = Vel;
		*/
		GetComponentInChildren<EllipsoidParticleEmitter> ().worldVelocity = transform.right;
	}


	void OnTriggerStay(Collider coll) {
		if(coll.gameObject.tag == "marble") {
			Jukebox.Instance.playASound("CONVEYOR");
			Rigidbody cRigid = coll.attachedRigidbody;
			Vector3 vec = transform.right * power * (maxSpeed - cRigid.velocity.magnitude)/maxSpeed;
			cRigid.AddForce(vec);
			
		}
	}

}
