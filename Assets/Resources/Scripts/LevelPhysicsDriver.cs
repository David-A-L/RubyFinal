using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class LevelManager : MonoBehaviour {
	/// <summary>
	/// Level physics driver. Use this to drive 
	/// </summary>
	public class LevelPhysicsDriver : MonoBehaviour {

		public LevelManager myParent;

		/*public LevelPhysicsDriver (LevelManager Parent) {
			myParent = Parent;
		}*/

		// Use this for initialization
		void Start () {
			
		}

		void FixedUpdate () {
			//Could switch on antigravity, zero gravity, etc...
			for(int i = 0; i < myParent.ballList.Count; ++i){
				GameObject ball_GO = myParent.ballList[i];
				if(ball_GO == null) {
					myParent.ballList.Remove(ball_GO);
				}
				else {
					BallScript bs = ball_GO.GetComponent<BallScript>();
					Rigidbody br = ball_GO.GetComponent<Rigidbody>();
					br.AddForce(bs.ballGravDir * bs.gravStr);
				}
			}
		}

	}
}
