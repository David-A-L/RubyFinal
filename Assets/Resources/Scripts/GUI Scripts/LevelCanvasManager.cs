using UnityEngine;
using System.Collections;

public class LevelCanvasManager : MonoBehaviour {
	/*
	 * Script to manage and control ui Elements for this scene
	 * 
	 * 
	 */ 
	 
	public void changeColor(){
		LevelManager.keyCommandList.changeColor(true);
		
	}
	public void changePlatformType(){
		LevelManager.keyCommandList.changePlatformType(true);
		
	}
	public void activateLevel(){
		LevelManager.keyCommandList.activateLevel(true);
		
	}
	public void resetLevel(){
		LevelManager.keyCommandList.resetLevel(true);
		
	}
	public void gotoMainMenu(){
		LevelManager.keyCommandList.gotoMainMenu(true);
		
	}
	public void toggleHelpText(){
		LevelManager.keyCommandList.toggleHelpText(true);
	}

}
