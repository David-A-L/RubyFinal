using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {
	
	public Vector3 ballGravDir;
	Transform gravArrow;
	public float gravStr = 9.8f;
	public string gravityGroup = "grav_dir_red";
	public bool isMeldable;
	public float rewardMult;
	public float reward;
	public BuilderID playerID;
	
	// Use this for initialization
	void Start () {
		gravArrow = GameObject.FindGameObjectWithTag (gravityGroup).transform;
		//not sure what's this ballGravDir being used for, have to confer w/David
		ballGravDir = gravArrow.up;
		//ballGravDir = Vector3.down;
		rewardMult = 1.5f;
		reward = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		ballGravDir = gravArrow.up;
		Debug.DrawRay (transform.position, ballGravDir);
	}
	
	//maybe switch to on collision?
	void OnCollisionEnter(Collision ballColl) {
		print ("Enterred collision");
		handleMeld(ballColl.collider);
	}
	
	void handleMeld(Collider coll) {
		print ("Melding");
		if (coll.tag != "marble" || !coll.gameObject.activeInHierarchy) {return;}

		BallScript colBallScpt = coll.gameObject.GetComponent<BallScript>();
		if(!colBallScpt.isMeldable) {return;}
		

		float volBallThis = gameObject.transform.localScale.x;
		float volBallCold = coll.gameObject.transform.localScale.x;

		//will return the scale for a ball whose vol would equal the sum of ball A and B
		float newBallScale = Mathf.Pow(
			Mathf.Pow(volBallThis, 3) + //scale 1 cubed 
			Mathf.Pow(volBallCold,3),  // plus scale 2 cubed
			(1f/3f) // and take cube root.
		);

		bool sameVol = volBallThis == volBallCold;
		bool largerVol = volBallThis > volBallCold;

		bool samePlayer = gameObject.GetComponent<Renderer>().material.color == coll.gameObject.GetComponent<Renderer>().material.color;
		bool lowerInstanceID = GetInstanceID() < colBallScpt.GetInstanceID();

		if(largerVol || (samePlayer && sameVol && lowerInstanceID)) {
		
			//Increase volume of marble
			gameObject.transform.localScale = new Vector3(newBallScale,newBallScale,newBallScale);
			
			//Increase mass of marble, more realistic collisions
			gameObject.GetComponent<Rigidbody>().mass = newBallScale;	
			
			reward = (reward + colBallScpt.reward)*rewardMult;

			colBallScpt.gameObject.SetActive(false);
			Destroy(colBallScpt.gameObject);
			Jukebox.Instance.playASound ("MELD");
		}
	}

}

