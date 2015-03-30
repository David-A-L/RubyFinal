using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	public Vector3 ballGravDir;
	public float gravStr = 9.8f;
	public string gravityGroup = "grav_dir_red";
	public bool isMeldable;
	public static float baseSphereVolume;
	Transform gravTransform;

	// Use this for initialization
	void Start () {
		baseSphereVolume = volumeSphere(0.5f);
		gravTransform = GameObject.FindGameObjectWithTag (gravityGroup).transform;
		ballGravDir = gravTransform.up;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Debug.DrawRay (transform.position, ballGravDir);
		ballGravDir = gravTransform.up;
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
					print ("Destroying ball with id" + ballTwoColl.GetInstanceID());
					Destroy(ballTwoColl.gameObject);
				}
			}
		}
	}
	
	
	static float volumeSphere(SphereCollider ball) {
		return (4f/3f) * Mathf.PI * Mathf.Pow(ball.radius,3);
	}
	static float volumeSphere(float rad) {
		return (4f/3f) * Mathf.PI * Mathf.Pow(rad,3);
	}
	
	static float radiusSphere(float volume) {
		return Mathf.Pow ((3f*volume)/(4f*Mathf.PI),1f/3f);
	}
	static float multipleScoreMeldedSphere(SphereCollider ball) {
		float volBall = volumeSphere(ball);
		float ratio = volBall / baseSphereVolume;
		return Mathf.Pow (2f,ratio-1f);
		
	}
	
}
