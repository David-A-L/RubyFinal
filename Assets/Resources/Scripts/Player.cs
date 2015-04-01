using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {
	public PlatformType currentPlatformType; //to keep history of player's platform type
	public List <BarScript> playerPowerBars = new List<BarScript>();
	public Material material;
	public GameObject defaultLine;
	public GameObject conveyorLine;
	public int ID;
	public int gameScore;
	public int levelScore;
	public DrawPlatform_Alt platformDrawer;

	public LevelManager levelManager;

	void Start(){
			
	}

	//public int lives;

	//ID MUST BE 1-4
	public Player(int _ID) {
		ID = _ID;
		gameScore = 0;
		levelScore = 0;
		resourceHelper ();
		currentPlatformType = PlatformType.DEFAULT;
		platformDrawer = Camera.main.gameObject.AddComponent<DrawPlatform_Alt>();
	}

	private void resourceHelper(){

		conveyorLine = (GameObject)Resources.Load ("Prefabs/cnvrPfrm");

		switch (ID) {
			case 0: 
				material = (Material)Resources.Load ("UX/SimpleMaterials/player1");
				defaultLine = (GameObject)Resources.Load ("Prefabs/linePlayer1");
				break;
			case 1:
				material = (Material)Resources.Load ("UX/SimpleMaterials/player2");
				defaultLine = (GameObject)Resources.Load ("Prefabs/linePlayer2");
				break;
			case 2: 
				material = (Material)Resources.Load ("UX/SimpleMaterials/player3");
				defaultLine = (GameObject)Resources.Load ("Prefabs/linePlayer3");
				break;
			case 3:
				material = (Material)Resources.Load ("UX/SimpleMaterials/player4");
				defaultLine = (GameObject)Resources.Load ("Prefabs/linePlayer4");
				break;
		}
	}

	public BarScript getCurrentPowerBar (){
		return playerPowerBars [(int)currentPlatformType];
	}
	
	public void changeToNextPlatformType(PlatformType pt){
		currentPlatformType = pt;
	}

	public void resetLevelScore () {
		levelScore = 0;
	}

	public void addToLevelScore(int addToScore) {
		gameScore += addToScore;
	}

	public void addToGameScore(int addToScore) {
		levelScore += addToScore;
	}

}
