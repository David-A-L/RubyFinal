using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class platformState{
	public GameObject platObj;
	public PlatformType platType;
}

public class Toolbox{
	static public float defaultAmt = 25;
	static public float conveyorAmt = 10;
	public List<float> baseAmounts;

	public Toolbox(){
		baseAmounts = new List<float>();
		baseAmounts.Add (defaultAmt);
		baseAmounts.Add (conveyorAmt);
	}

}
//Player in Level corresponds to the fundamental things necessary to keep track of for a given player WITHIN a level
//The LevelManager uses a Static List<playerInLevel> to keep instances of this class for each player in the game.
//This way, if a player chooses to undo, redo, or any other action specific to a level, the Level Manager can update 
//the state for that player Use this wisely and don't add unnecessary information to this class. Remember that level-wide 
//actions such as reset will require iterating through the entire allPlayers List and correctly performing the action on 
//each instance of this class. 
public class playerInLevel{
	public List<platformState> platformsLaid = new List<platformState>();
	public List<platformState> platformsUndid = new List<platformState>();

}


public partial class LevelManager : MonoBehaviour {
	//UX
//	static private List<GameObject> powerBars;

	//PHYSICS
	LevelPhysicsDriver physicsDriver;

	//PLAYER MANAGEMENT VARIABLES AND FUNCTIONS
	static private int currentTurn = 0;
	public void tick(){
		++currentTurn;
		refreshShowingBar ();
		//Query Gm Mngr, get list of all power bars, index on curPlayNum, index further on platType (player history for plat type), and activate
	}

	public Player getCurrentPlayer(){return GameManager.Instance.players [getCurrentPlayerNum ()];}
	public int getCurrentPlayerNum(){return (currentTurn % GameManager.Instance.numPlayers);}
	static private List<playerInLevel> allPlayers;

	//DICTIONARIES	
	Dictionary<PlatformType, bool> EnabledDict = new Dictionary<PlatformType, bool> ();//ASSOCIATE CORRECT ENABLING ?

	//LEVEL STATE FUNCTIONS	
	Toolbox toolbox;
	public enum LevelState{PAUSED, PLANNING, RUNNING};
	public static LevelState curLevelState = LevelState.PLANNING;

	//Container for all balls on level
	public List<GameObject> ballList = new List<GameObject> ();

	//Scoring
	static int scoreMultiplier = 10; 


	//ONCE, ADD MANAGEMENT INSTANCES FOR EACH PLAYER
	[RuntimeInitializeOnLoadMethod]
	static void OnRuntimeMethodLoad (){
		allPlayers = new List<playerInLevel> ();
	}

	// Use this for initialization
	void Start () {

		toolbox = new Toolbox ();
		refreshShowingBar ();
		Instantiate ((GameObject)Resources.Load ("Prefabs/Indicator"));
		//Add each ball to the ball list. 
		GameObject[] ballArr = GameObject.FindGameObjectsWithTag("marble");
		foreach (GameObject b in ballArr) {ballList.Add(b);}

		for (int i = 0; i < GameManager.Instance.numPlayers; ++i) {
			playerInLevel player_to_add = new playerInLevel ();
			allPlayers.Add(player_to_add);
		}
		
		//Have to add the script as a Component to the current gameObject (unity sucks)
		//Hack
		EnabledDict [PlatformType.DEFAULT] = true;
		EnabledDict [PlatformType.CONVEYOR] = true;

		physicsDriver = gameObject.AddComponent<LevelPhysicsDriver> ();
		physicsDriver.myParent = this;
		physicsDriver.enabled = false;
		
	}

	public PlatformType nextValidPlatformType(PlatformType curPlatType) {

		int nextPlatNum = (int)curPlatType;
		int numTypePlatforms = Enum.GetValues (typeof(PlatformType)).Length;
		do{
			nextPlatNum++;
			nextPlatNum %= numTypePlatforms;
		} while(!EnabledDict[(PlatformType)nextPlatNum]);

		return (PlatformType)nextPlatNum;
	}
	
	//CALL THIS WHEN YOU'RE DONE DRAWING AND SETTING A NEW PLATFORM
	public void AddPlatform(GameObject platform){
		int curPlayerID = getCurrentPlayerNum ();
		Player curPlayer = getCurrentPlayer();
		PlatformType curPlatType = curPlayer.currentPlatformType;

		platformState platformStateToAdd = new platformState ();
		platformStateToAdd.platType = curPlatType;
		platformStateToAdd.platObj = platform;
		allPlayers[curPlayerID].platformsLaid.Add(platformStateToAdd);

		float sizeToSet = calculateBarSizeForPlayerAndType (curPlayerID, curPlatType);
		curPlayer.getCurrentPowerBarScript ().setSize (sizeToSet);

		//Cleaning up
		foreach (platformState ps in allPlayers[curPlayerID].platformsUndid){GameObject.Destroy(ps.platObj);}
		allPlayers[curPlayerID].platformsUndid.Clear ();
	}
	
	//CALL THIS WHEN YOU WISH TO UNDO PLATFORMS PREVIOUSLY DRAWN
	public void undoDrawPlatform (){
		int curPlayerID = getCurrentPlayerNum ();
		int num_platforms_laid = allPlayers[curPlayerID].platformsLaid.Count;
		if (num_platforms_laid == 0) {return;}

		platformState platformStateToUndo = allPlayers[curPlayerID].platformsLaid[num_platforms_laid - 1];
		platformStateToUndo.platObj.SetActive(false);
		allPlayers[curPlayerID].platformsUndid.Add (platformStateToUndo);
		allPlayers[curPlayerID].platformsLaid.RemoveAt (num_platforms_laid-1);

		getCurrentPlayer ().changeToPlatformType (platformStateToUndo.platType);

		float sizeToSet = calculateBarSizeForPlayerAndType (curPlayerID, platformStateToUndo.platType);
		getCurrentPlayer().getCurrentPowerBarScript ().setSize (sizeToSet);

		refreshShowingBar ();
	}
	
	public void RedoDrawPlatform (){
		int curPlayerID = getCurrentPlayerNum ();
		int num_platforms_undid = allPlayers[curPlayerID].platformsUndid.Count;
		if (num_platforms_undid == 0) {return;}

		platformState platformStateToRedo = allPlayers[curPlayerID].platformsUndid[num_platforms_undid - 1];
		platformStateToRedo.platObj.SetActive (true);

		allPlayers[curPlayerID].platformsLaid.Add (platformStateToRedo);

		allPlayers[curPlayerID].platformsUndid.RemoveAt (num_platforms_undid - 1);
		getCurrentPlayer ().changeToPlatformType (platformStateToRedo.platType);

		float sizeToSet = calculateBarSizeForPlayerAndType (curPlayerID, platformStateToRedo.platType);
		getCurrentPlayer().getCurrentPowerBarScript ().setSize (sizeToSet);
		refreshShowingBar ();
	}
	
	
	// Update is called once per frame
	void Update () {
		if (DrawPlatform_Alt.curState == DrawPlatform_Alt.DrawState.NONE) {
			if (Input.GetButton ("ctrl") && Input.GetButton ("shift") && Input.GetKeyDown (KeyCode.Z)) {RedoDrawPlatform ();} 
			else if ((Input.GetButton ("ctrl") && Input.GetKeyDown (KeyCode.Z))|| Input.GetKeyDown (KeyCode.U)) {undoDrawPlatform ();}
		}
		if (Input.GetKeyUp (KeyCode.S)) {ResetLevel();}
		if (Input.GetKeyUp(KeyCode.A)){ActivateLevel();}
		if (Input.GetKey (KeyCode.Backspace)) {
			GameManager.Instance.disableAllBars();
			Application.LoadLevel("_scene_main_menu");
		}
		
		//score hacks
		string msg = @"Scores: ";
		foreach(Player plyr in GameManager.Instance.players){
			msg += plyr.ID + " " + plyr.levelScore + " ";
		}
		GameManager.Instance.playerScoreText.GetComponent<Text>().text = msg;
			
	}

	void ActivateLevel(){
		physicsDriver.enabled = true;
	}
	
	public void ResetLevel(){ //this will generally be used in a single player context, consider a separate multi-player reset
		GameManager.Instance.disableAllBars ();
		currentTurn = 0;
		resetPlayerScores();
		preservePlatforms ();
		Application.LoadLevel (Application.loadedLevel);
		updateShowingBar ();
	}
	
	public void preservePlatforms(){
		allPlayers.ForEach(delegate (playerInLevel individual_player){
			individual_player.platformsLaid.ForEach(delegate(platformState ps){UnityEngine.Object.DontDestroyOnLoad(ps.platObj);});
			individual_player.platformsUndid.ForEach(delegate(platformState ps){UnityEngine.Object.DontDestroyOnLoad(ps.platObj);});
		});
	}

	static public void deleteAllPlatforms(){
		allPlayers.ForEach(delegate (playerInLevel individual_player){
			individual_player.platformsLaid.ForEach(delegate(platformState ps){UnityEngine.Object.Destroy(ps.platObj);});
			individual_player.platformsUndid.ForEach(delegate(platformState ps){UnityEngine.Object.Destroy(ps.platObj);});
		});
	}
	private void resetPlayerScores() {
		for(int i = 0; i < GameManager.Instance.players.Count; ++i) {
			GameManager.Instance.players[i].levelScore = 0;
		}
	}

	//Disable all power bars and then activate the current one
	public void refreshShowingBar(){
		GameManager.Instance.disableAllBars ();
		getCurrentPlayer().getCurrentPowerBar ().SetActive (true);
	}

	//update the current(/should be showing) power bar with the correct value to show
	public void updateShowingBar(){
		Player curPlayer = getCurrentPlayer ();
		PlatformType pt = curPlayer.currentPlatformType;
		BarScript showingBarScript = curPlayer.getCurrentPowerBarScript ();
		showingBarScript.setSize (	calculateBarSizeForPlayerAndType (getCurrentPlayerNum(), pt)	);
	}

	public void endLevel(){
		currentTurn = 0;
		deleteAllPlatforms();
		GameManager.Instance.disableAllBars ();
		resetPlayerScores();
		
		if (Application.loadedLevel == GameManager.Instance.numLevels-1) {Application.LoadLevel(0);}
		Application.LoadLevel(Application.loadedLevel + 1);
	}

	public float calculateBarSizeForPlayerAndType(int playerNum, PlatformType pt){
		float curAmountUsed = 0f;
		allPlayers [playerNum].platformsLaid.ForEach (delegate (platformState ps) {
			if (ps.platType == pt) {curAmountUsed += ps.platObj.transform.localScale.x;}
		});
		return 1f - (curAmountUsed / toolbox.baseAmounts[(int) pt]);
	}

	public float calculateBarSizeForCurrentEverything_plus_PlatformP(GameObject p){
		PlatformType pt = getCurrentPlayer ().currentPlatformType;

		float currentUsed = calculateBarSizeForPlayerAndType (getCurrentPlayerNum(), pt);
		currentUsed -= (p.transform.localScale.x / toolbox.baseAmounts [(int)pt]);
		return currentUsed;
	}


	public void updatePlayerScore(GameObject marble) {
		int pointsGained = (int)(marble.GetComponent<BallScript>().reward * scoreMultiplier);
		GameManager.Instance.players [(int)marble.GetComponent<BallScript>().playerID].levelScore += pointsGained;
	}

}
