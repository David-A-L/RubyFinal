using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class LevelManager : MonoBehaviour {
	/// <summary>
	/// Level physics driver. Use this to drive 
	/// </summary>
	public class LevelPhysicsDriver : MonoBehaviour {
		
		private LevelManager myParent;

		public LevelPhysicsDriver (LevelManager Parent) {
			myParent = Parent;
		}
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
