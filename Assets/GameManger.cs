using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager {
	
	public List<Player> players = new List<Player> ();

	//List[playerID][playerPowerBars] to Game Objects, holds instances of UI power bars
	public List<List<GameObject> > allPlayersPowerBars = new List<List<GameObject>>();

	public GameObject levelCanvas;

	public int numPlayers = 4; //make sure to call set up Players with right number
	public int completedLevels;
	public int currentLevel;
	public int numLevels = 20;
	public GameObject playerScoreText;
	
	private static GameManager instance = new GameManager();

	// initialize game manager here. Do not reference GameObjects here (i.e. GameObject.Find etc.)
	// because the game manager will be created before the objects
	private GameManager() {
		completedLevels = 0;
		levelCanvas = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/LevelCanvas", typeof(GameObject))) as GameObject;
		playerScoreText = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/PlayerScoreText", typeof(GameObject))) as GameObject;
		playerScoreText.transform.SetParent(levelCanvas.transform);
		playerScoreText.transform.localPosition = new Vector3(80f,120f,0);
		
		
		//HACK
		setUpPlayers (4);

		UnityEngine.Object.DontDestroyOnLoad (levelCanvas);
		
		//Initialize the InputManager
		KeyCommandList.Instance.init();
	}    

	//call GameManager.Instance.<Function to Call>() to communicate with game mngr
	public static GameManager Instance {
		get { return instance; }
	}

	private void setUpPlayers(int totPlayers){
		numPlayers = totPlayers;
		for (int i = 0; i < numPlayers; ++i) {
			Player playerToAdd = new Player((BuilderID)i);
			List<GameObject> listForPlayer = new List<GameObject> ();//for each player, make a new list to hold each power bar

			for (int j = 0; j < Enum.GetValues (typeof(PlatformType)).Length; ++j){	//for each type of power bar
				GameObject powBar = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/PowerBar", typeof(GameObject))) as GameObject;

				BarScript barScript = powBar.gameObject.GetComponentsInChildren<BarScript>()[0];//get the barScript handle for it

				barScript.personalizeToPlayer(playerToAdd,(PlatformType)j);	//personalize it for the player
				playerToAdd.playerPowerBarScripts.Add(barScript);	//give the player a handle to the script
				listForPlayer.Add(powBar);	//keep a handle to the actual power bar in the list of power bars for the player
				powBar.transform.SetParent(levelCanvas.transform);
				Vector2 newPos = new Vector2(0f, -180f);
				powBar.transform.localPosition = newPos;
			}

			playerToAdd.playerPowerBars = listForPlayer;//add this list for each player
			players.Add(playerToAdd);//add the fully furnished handle to the player to the list of players
		}
	}

	public void disableAllBars(){
		players.ForEach(delegate (Player player){
				player.playerPowerBars.ForEach(delegate (GameObject powerBar) {
					powerBar.SetActive(false);
				});
		});
	}
	
	//Call this when you finish a level
	public void finishLevel(){
		++completedLevels;
		currentLevel = Application.loadedLevel;
		++currentLevel;
		if (currentLevel == numLevels){--currentLevel;}
		Application.LoadLevel (currentLevel);
	}


	public bool newLevel = true;
	
	//This could probably be a private function, I believe the gameManager will be the only one calling it
	public void resetGame(){
		completedLevels = 0;
	}

}
