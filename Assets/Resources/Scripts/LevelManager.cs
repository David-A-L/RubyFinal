﻿using UnityEngine;
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
		//DrawPlatform_Alt.PoolDict [undoPlatform.platformType].changeSize (DrawPlatform_Alt.difficulty * undoPlatform.gameObj.transform.localScale.x);
		GameObject.FindGameObjectWithTag("pool_default").GetComponent<BarScript>().changeSize (DrawPlatform_Alt.difficulty * undoPlatform.gameObj.transform.localScale.x);
		undoList.Add (undoPlatform);
		platforms.RemoveAt (platforms.Count - 1);
		//INSTANTIATE/RETURN CALL
	}
	
	public void RedoDrawPlatform (){
		if (undoList.Count == 0) {return;}
		platformState redoPlatform = undoList[undoList.Count - 1];
		undoList.RemoveAt (undoList.Count - 1);
		redoPlatform.gameObj.SetActive (true);
		//DrawPlatform_Alt.PoolDict [redoPlatform.platformType].changeSize (DrawPlatform_Alt.difficulty * -redoPlatform.gameObj.transform.localScale.x);
		GameObject.FindGameObjectWithTag("pool_default").GetComponent<BarScript>().changeSize (DrawPlatform_Alt.difficulty * -redoPlatform.gameObj.transform.localScale.x);
		platforms.Add (redoPlatform);
		//INSTANTIATE/RETURN CALL
	}
	
	
	// Update is called once per frame
	void Update () {
		if (DrawPlatform_Alt.curState == DrawPlatform_Alt.DrawState.NONE) {
			if (Input.GetButton ("ctrl") && Input.GetButton ("shift") && Input.GetKeyDown (KeyCode.Z)) {
				RedoDrawPlatform ();
			} else if ((Input.GetButton ("ctrl") && Input.GetKeyDown (KeyCode.Z))
			           || Input.GetKeyDown (KeyCode.U)) {
				undoDrawPlatform ();
			}
		}
		if (Input.GetKeyUp (KeyCode.S)) {ResetLevel();}
	}
	
	public void ResetLevel(){
		preservePlatforms ();
		Application.LoadLevel (Application.loadedLevel);
	}
	
	public void preservePlatforms(){
		platforms.ForEach (delegate(platformState ps){Object.DontDestroyOnLoad(ps.gameObj);});
		undoList.ForEach (delegate(platformState ps){Object.DontDestroyOnLoad(ps.gameObj);});
	}
	
	public void endLevel(){
		if (Application.loadedLevel == GameManager.Instance.numLevels-1) {Application.LoadLevel(0);}
		Application.LoadLevel(Application.loadedLevel + 1);
	}
}
