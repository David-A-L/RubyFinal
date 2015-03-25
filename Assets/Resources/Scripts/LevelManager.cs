using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class platformState{
	//		public platformState(GameObject platform, DrawPlatform_Alt.PlatformType ptype){
	//			gameObj = platform;
	//			platformType = ptype;
	//		}
	public GameObject gameObj;
	public DrawPlatform_Alt.PlatformType platformType;
};


public partial class LevelManager : MonoBehaviour {

	static private List<platformState> platforms;
	static private List<platformState> undoList;
	
	[RuntimeInitializeOnLoadMethod]
	static void OnRuntimeMethodLoad (){
		platforms = new List<platformState>();
		undoList = new List<platformState>();
	}


	//all balls in level, updated and maintained solely by level manager
	//
	//public ArrayList<>
	LevelPhysicsDriver physicsDriver;

	// Use this for initialization
	void Start () {
//		physicsDriver = new LevelPhysicsDriver (this);
	}

	//CALL THIS WHEN YOU'RE DONE DRAWING AND SETTING A NEW PLATFORM
	public void AddPlatform(GameObject platform, DrawPlatform_Alt.PlatformType ptype){
		platformState newPlatform = new platformState();
		newPlatform.gameObj = platform;
		newPlatform.platformType = ptype;
		platforms.Add (newPlatform);
		undoList.Clear ();
	}

	//CALL THIS WHEN YOU WISH TO UNDO PLATFORMS PREVIOUSLY DRAWN
	public void undoDrawPlatform (){
		if (platforms.Count == 0) {return;}
		platformState undoPlatform = platforms[platforms.Count - 1];
		undoPlatform.gameObj.SetActive(false);
		undoList.Add (undoPlatform);
		platforms.RemoveAt (platforms.Count - 1);
		//INSTANTIATE/RETURN CALL
	}

	public void RedoDrawPlatform (){
		if (undoList.Count == 0) {return;}
		platformState redoPlatform = undoList[undoList.Count - 1];
		undoList.RemoveAt (undoList.Count - 1);
		redoPlatform.gameObj.SetActive (true);
		platforms.Add (redoPlatform);
		//INSTANTIATE/RETURN CALL
	}


	// Update is called once per frame
	void Update () {
		if (Input.GetButton("ctrl") &&  Input.GetButton("shift") && Input.GetKeyDown(KeyCode.Z) ){RedoDrawPlatform();}
		else if (Input.GetButton("ctrl") && Input.GetKeyDown(KeyCode.Z)){undoDrawPlatform();}
	}
}
