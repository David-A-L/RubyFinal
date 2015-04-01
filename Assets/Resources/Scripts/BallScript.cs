using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {
	
	public Vector3 ballGravDir;
	public float gravStr = 9.8f;
	public string gravityGroup = "grav_dir_red";
	public bool isMeldable;
	public static float baseSphereVolume;
	public float rewardMult;
	public float reward;
	public BuilderID playerID;
	
	// Use this for initialization
	void Start () {
		baseSphereVolume = volumeSphere(0.5f);
		
		//not sure what's this ballGravDir being used for, have to confer w/David
		//ballGravDir = GameObject.FindGameObjectWithTag (gravityGroup).transform.up;
		ballGravDir = Vector3.down;
		rewardMult = 1.5f;
		reward = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay (transform.position, ballGravDir);
	}
	
	//maybe switch to on collision?
	void OnTriggerEnter(Collider ballColl) {
		handleMeld(ballColl);
	}
	
	void handleMeld(Collider coll) {
		if(coll.tag == "marble" && coll.isTrigger && coll.gameObject.activeInHierarchy) {
			
			SphereCollider thisBall = gameObject.GetComponent<SphereCollider>();
			SphereCollider colidingBall = coll.GetComponent<SphereCollider>();
			BallScript colBallScpt = colidingBall.GetComponent<BallScript>();
			
			if(colBallScpt.isMeldable) {
				
				float volBallThis = volumeSphere(thisBall);
				float volBallCold = volumeSphere(colidingBall);
				float newBallRadius = radiusSphere(volBallThis + volBallCold);
				
				bool samePlayer = thisBall.GetComponent<Renderer>().material.color == coll.gameObject.GetComponent<Renderer>().material.color;
				bool sameVol = volBallThis == volBallCold;
				bool lowerInstanceID = GetInstanceID() < colBallScpt.GetInstanceID();
				bool largerVol = volBallThis > volBallCold;
				
				if(largerVol || (samePlayer && sameVol && lowerInstanceID)) {
					
					gameObject.transform.localScale = new Vector3(newBallRadius,newBallRadius,newBallRadius);	
					
					reward = (reward + colBallScpt.reward)*rewardMult;
					
					SphereCollider[] sColls = GetComponents<SphereCollider>();
					foreach(SphereCollider nxt in sColls) {
						nxt.radius = newBallRadius;
					}
					colBallScpt.gameObject.SetActive(false);
					Destroy(colBallScpt.gameObject);
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

	//public static int multipleMeldedSphere(GameObject marble) {
	//	SphereCollider sColl = marble.GetComponent<SphereCollider> ();
	//	return (int)(volumeSphere (sColl) / baseSphereVolume);
	//}
	
}