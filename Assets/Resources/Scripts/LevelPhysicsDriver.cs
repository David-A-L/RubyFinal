﻿using UnityEngine;
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
			foreach (GameObject ball_GO in myParent.ballList){
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
