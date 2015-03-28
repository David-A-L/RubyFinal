﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager {
	
	public List<Player> players;
	public Material[] playerMaterials = new Material[4];
	public int numPlayers = 2;
	public int completedLevels;
	public int currentLevel;
	public int numLevels = 20;
	private int numLives;
	private int numDeaths; //when death == lives, game restarts from beginning
	 
	private List<Vector3> obstaclePos;
	public bool seedRand;

	private static GameManager instance = new GameManager();

	// initialize game manager here. Do not reference GameObjects here (i.e. GameObject.Find etc.)
	// because the game manager will be created before the objects
	private GameManager() {

		playerMaterials[0] = (Material)Resources.Load ("UX/SimpleMaterials/player1");
		playerMaterials[1] = (Material)Resources.Load ("UX/SimpleMaterials/player2");
		playerMaterials[2] = (Material)Resources.Load ("UX/SimpleMaterials/player3");
		playerMaterials[3] = (Material)Resources.Load ("UX/SimpleMaterials/player4");

		Player player1 = new Player (playerMaterials[0]);

		numLives = 4;
		numDeaths = 0;
		completedLevels = 0;
		seedRand = true;
	}    

	//call GameManager.Instance.<Function to Call>() to communicate with game mngr
	public static GameManager Instance {
		get { return instance; }
	}

	//Call this when you finish a level
	public void finishLevel(){
		++completedLevels;
		currentLevel = Application.loadedLevel;
		++currentLevel;
		if (currentLevel == numLevels){--currentLevel;}
		Application.LoadLevel (currentLevel);
	}

	//Call this when the player wishes to restart the level
	public void resetLevel(List<Vector3> obstPositions){
		obstaclePos = obstPositions;
		seedRand = false;
	}

	public bool newLevel = true;
	
	//Call this when randomly generating a level to see if the level is a repeat
	//if so, skip random generation, and simply use the values returned
	public List<Vector3> levelRepeat(){
		if (seedRand) {return null;}
		else {
			seedRand = true;
			return obstaclePos;
		}
	}

	//Call this when the player dies, followed by resetLevel
	//If the player has no lives left, then the call to resetLevel
	//never actually go through and so there's nothing to worry about
	public void die(List<Vector3> obstPositions){
		++numDeaths; 
		if (numDeaths == numLives) {
			resetGame ();
			return;
		}
		//else, load scene, with completedLevels as seed for how level should be set up
	}

	//This could probably be a private function, I believe the gameManager will be the only one calling it
	public void resetGame(){
		numDeaths = 0;
		completedLevels = 0;
	}

}
