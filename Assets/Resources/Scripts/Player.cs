using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player{
	public PlatformType currentPlatformType; //to keep history of player's platform type
	public List <BarScript> playerPowerBarScripts = new List<BarScript>();
	public List <GameObject> playerPowerBars = new List<GameObject>();

	public Material material;
	public Material particleMaterial;
	public GameObject defaultLine;
	public GameObject conveyorLine;
	public BuilderID ID;
	public int gameScore;
	public int levelScore;
	public DrawPlatform_Alt platformDrawer;

	public LevelManager levelManager;

	void Start(){
			
	}
	//public int lives;

	//ID MUST BE 1-4
	public Player(BuilderID _ID) {
		ID = _ID;
		gameScore = 0;
		levelScore = 0;
		resourceHelper ();
		currentPlatformType = PlatformType.DEFAULT;
//		platformDrawer = Camera.main.gameObject.AddComponent<DrawPlatform_Alt>();
	}

	private void resourceHelper(){

		conveyorLine = (GameObject)Resources.Load ("Prefabs/cnvrPfrm");

		switch (ID) {
			case BuilderID.PLAYER1: 
				material = (Material)Resources.Load ("UX/SimpleMaterials/player1");
				defaultLine = (GameObject)Resources.Load ("Prefabs/linePlayer1");
				particleMaterial = (Material)Resources.Load ("UX/ParticleMaterials/p1_particles");
			break;

				case BuilderID.PLAYER2:
				material = (Material)Resources.Load ("UX/SimpleMaterials/player2");
				defaultLine = (GameObject)Resources.Load ("Prefabs/linePlayer2");
				particleMaterial = (Material)Resources.Load ("UX/ParticleMaterials/p2_particles");
			break;

				case BuilderID.PLAYER3: 
				material = (Material)Resources.Load ("UX/SimpleMaterials/player3");
				defaultLine = (GameObject)Resources.Load ("Prefabs/linePlayer3");
				particleMaterial = (Material)Resources.Load ("UX/ParticleMaterials/p3_particles");
			break;
				case BuilderID.PLAYER4:
				material = (Material)Resources.Load ("UX/SimpleMaterials/player4");
				defaultLine = (GameObject)Resources.Load ("Prefabs/linePlayer4");
				particleMaterial = (Material)Resources.Load ("UX/ParticleMaterials/p4_particles");
			break;
		}
	}
	

	public BarScript getCurrentPowerBarScript (){
		return playerPowerBarScripts [(int)currentPlatformType];
	}

	public BarScript getPowerBarScriptForType(PlatformType pt){
		return playerPowerBarScripts [(int)pt];
	}

	public GameObject getCurrentPowerBar (){
		return playerPowerBars[(int)currentPlatformType];
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
