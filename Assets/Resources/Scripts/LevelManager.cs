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
	//paused/playing should go in gamemanager?
	public enum LevelState{PAUSED, PLANNING, RUNNING};
	public static LevelState curLevelState = LevelState.PLANNING;

	static private List<platformState> platforms;
	static private List<platformState> undoList;
	public List<GameObject> ballList = new List<GameObject> ();

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
		GameObject[] ballArr = GameObject.FindGameObjectsWithTag("marble");
		foreach (GameObject b in ballArr) {
			ballList.Add(b);
		}

		//Have to add the script as a Component to the current gameObject (unity sucks)
		physicsDriver = gameObject.AddComponent<LevelPhysicsDriver> ();
		physicsDriver.myParent = this;
		physicsDriver.enabled = false;
	}
	
	//CALL THIS WHEN YOU'RE DONE DRAWING AND SETTING A NEW PLATFORM
	public void AddPlatform(GameObject platform, DrawPlatform_Alt.PlatformType ptype){
		platformState newPlatform = new platformState();
		newPlatform.gameObj = platform;
		newPlatform.platformType = ptype;
		platforms.Add (newPlatform);

		//Cleaning up
		foreach (platformState p in undoList){
			GameObject.Destroy(p.gameObj);
		}
		undoList.Clear ();
	}
	
	//CALL THIS WHEN YOU WISH TO UNDO PLATFORMS PREVIOUSLY DRAWN
	public void undoDrawPlatform (){
		if (platforms.Count == 0) {return;}
		platformState undoPlatform = platforms[platforms.Count - 1];
		undoPlatform.gameObj.SetActive(false);
		DrawPlatform_Alt.PoolDict [undoPlatform.platformType].changeSize (DrawPlatform_Alt.difficulty * undoPlatform.gameObj.transform.localScale.x);
		//GameObject.FindGameObjectWithTag("pool_default").GetComponent<BarScript>().changeSize (DrawPlatform_Alt.difficulty * undoPlatform.gameObj.transform.localScale.x);
		undoList.Add (undoPlatform);
		platforms.RemoveAt (platforms.Count - 1);
		//INSTANTIATE/RETURN CALL
	}
	
	public void RedoDrawPlatform (){
		if (undoList.Count == 0) {return;}
		platformState redoPlatform = undoList[undoList.Count - 1];
		undoList.RemoveAt (undoList.Count - 1);
		redoPlatform.gameObj.SetActive (true);
		DrawPlatform_Alt.PoolDict [redoPlatform.platformType].changeSize (DrawPlatform_Alt.difficulty * -redoPlatform.gameObj.transform.localScale.x);
		//GameObject.FindGameObjectWithTag("pool_default").GetComponent<BarScript>().changeSize (DrawPlatform_Alt.difficulty * -redoPlatform.gameObj.transform.localScale.x);
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

		if (Input.GetKeyUp(KeyCode.A)){ActivateLevel();}

		if (Input.GetKey (KeyCode.Backspace)) {
			Application.LoadLevel("_scene_main_menu");
		}
		
	}

	void ActivateLevel(){

		/*foreach (GameObject ball in ballList) {
			Rigidbody mRigid = ball.GetComponent<Rigidbody> ();
			if (mRigid == null) {
				mRigid = ball.AddComponent<Rigidbody> ();
				mRigid.useGravity = false;
			}
			else {
				mRigid.useGravity = false;
			}
		}*/
		physicsDriver.enabled = true;
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
