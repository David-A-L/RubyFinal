using UnityEngine;
using System.Collections;

public class ConveyorPlatform : MonoBehaviour {

	public float slideVel;
	public float exitVelFraction;
	private bool facingRight;
	
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
