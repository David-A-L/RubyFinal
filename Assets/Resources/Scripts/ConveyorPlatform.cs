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
		Vector3 difference = transform.position - startPoint;
		Vector3 Vel = GetComponentInChildren<EllipsoidParticleEmitter>().worldVelocity;
		bool higherNow = transform.position.y > startPoint.y;
		Vel = higherNow ? difference : -difference;
		Vel = Vel.normalized;
		GetComponentInChildren<EllipsoidParticleEmitter>().worldVelocity = Vel;
	}
	
	//TODO CHANGE CLASS TO WORK W/ MULTIPLE BALLS/GRAVITY
	//SHOULD THE CONVEYOR ADD A FORCE IN THE DIRECTION IT IS CREATED (transform.right?)?
	
	void OnTriggerEnter(Collider coll) {
		if(coll.gameObject.tag == "marble") {
			Rigidbody marbleBody = coll.gameObject.GetComponent<Rigidbody>();
			marbleBody.velocity = Vector3.zero;
			marbleBody.useGravity = false;
			if (Physics.Raycast(marbleBody.position,Vector3.right, 1f)){
				facingRight = true;
			} else {
				facingRight = false;
			}
			
		}
	}
	
	void OnTriggerStay(Collider coll) {
		if(coll.gameObject.tag == "marble") {
		
			Vector3 newMarblePos = coll.gameObject.transform.position;
			
			float platformAdv = Time.deltaTime * slideVel;
			float zRotation = this.transform.rotation.z * Mathf.PI / 180f;
			float xDisplacment = Mathf.Abs(Mathf.Cos(zRotation) * platformAdv);
			float yDisplacment = Mathf.Abs(Mathf.Sin(zRotation) * platformAdv);

			newMarblePos.y += yDisplacment;
			if(facingRight){
				newMarblePos.x += xDisplacment;
			} else {
				newMarblePos.x -= xDisplacment;
			}
			
			coll.gameObject.transform.position = newMarblePos;
			
		}
	} 
	void OnTriggerExit(Collider coll) {
		if(coll.gameObject.tag == "marble") {
			Rigidbody marbleRBody = coll.gameObject.GetComponent<Rigidbody>();
			if(facingRight) {
				marbleRBody.velocity = new Vector3(exitVelFraction*slideVel,exitVelFraction*slideVel,0f);
			} else {
				marbleRBody.velocity = new Vector3(-exitVelFraction*slideVel,exitVelFraction*slideVel,0f);
			}
				
			marbleRBody.useGravity = true;
		}
	}
	
}
