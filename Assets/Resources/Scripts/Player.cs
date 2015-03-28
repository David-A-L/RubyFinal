using UnityEngine;
using System.Collections;

public class Player {

	public Material material;
	public int gameScore;
	public int levelScore;
	public DrawPlatform_Alt platformDrawer;

	//public int lives;
	
	public Player(Material materialIn) {
		material = materialIn;
		gameScore = 0;
		levelScore = 0;
		platformDrawer = Camera.main.gameObject.AddComponent<DrawPlatform_Alt>();
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
