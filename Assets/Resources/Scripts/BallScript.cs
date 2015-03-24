using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	private Vector3 ballGravity;
	public bool isMeldable;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//maybe switch to on collision?
	void OnTriggerEnter(Collider ballColl) {
		handleMeld(ballColl);
	}
	
	void handleMeld(Collider ballColl) {
		if(ballColl.tag == "marble" && ballColl.isTrigger && ballColl.gameObject.activeInHierarchy) {
			SphereCollider ballTwoColl = ballColl.GetComponent<SphereCollider>();
			SphereCollider thisColl = this.gameObject.GetComponent<SphereCollider>();
			
			BallScript adjMarble = ballTwoColl.GetComponent<BallScript>();
			if(adjMarble.isMeldable) {
				float volBallThis = volumeSphere(thisColl);
				float volBallTwo = volumeSphere(ballTwoColl);
				float newBallRadius = radiusSphere(volBallThis + volBallTwo);
				
				//"this" will become the larger ball, the other will be destroyed
				if((volBallThis == volBallTwo && thisColl.GetInstanceID() < ballTwoColl.GetInstanceID())
				   || volBallThis > volBallTwo) {
					this.gameObject.transform.localScale = new Vector3(newBallRadius,newBallRadius,newBallRadius);
					
					SphereCollider[] sColls = GetComponents<SphereCollider>();
					foreach(SphereCollider nxt in sColls) {
						nxt.radius = newBallRadius;
					}
					
					ballTwoColl.gameObject.SetActive(false);
					Destroy(ballTwoColl.gameObject);
				}
			}
		}
	}
	
	
	float volumeSphere(SphereCollider ball) {
		return (4f/3f) * Mathf.PI * Mathf.Pow(ball.radius,3);
	}
	float radiusSphere(float volume) {
		return Mathf.Pow ((3f*volume)/(4f*Mathf.PI),1f/3f);
	}
	
}
