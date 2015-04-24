using UnityEngine;
using System.Collections;

public class KeyCommandList : MonoBehaviour {

	private GameObject helpText;
	private static KeyCommandList instance;
	private LevelManager levelManager;
	private DrawPlatform_Alt drawPlatform_Alt;
	private bool isHelpTextEnabled;
	
	public KeyCommandList(){}
	
	public void changeColor(bool isFromGUI){
		if((Input.GetKeyUp(KeyCode.C) && Input.GetButton("shift")) || isFromGUI){
			drawPlatform_Alt.togglePlatforms(true);
		}
	}
	public void changePlatformType(bool isFromGUI){
		if((Input.GetKeyUp(KeyCode.C) && !Input.GetButton("shift")) || isFromGUI){
			drawPlatform_Alt.togglePlatforms(false);
		}
	}
	public void activateLevel(bool isFromGUI){
		if (Input.GetKeyUp(KeyCode.Space)|| isFromGUI){
			levelManager.ActivateLevel(isFromGUI);
		}
	}
	public void resetLevel(bool isFromGUI){
		if (Input.GetKeyUp (KeyCode.R)|| isFromGUI) {
			levelManager.ResetLevel();
		}
	}
	public void gotoMainMenu(bool isFromGUI){
		if (Input.GetKey (KeyCode.Backspace)|| isFromGUI) {
			GameManager.Instance.disableAllBars();
			Application.LoadLevel("_scene_main_menu");
		}
	}
	public void toggleHelpText(bool isFromGUI){
		if(Input.GetKeyDown(KeyCode.H)|| isFromGUI) {
			isHelpTextEnabled = !isHelpTextEnabled;
			helpText = GameObject.Find("help");
			helpText.GetComponent<Canvas>().enabled = isHelpTextEnabled;		
		}
	}

	void Update() {
		levelManager = GameObject.Find ("Main Camera").GetComponent<LevelManager>();
		drawPlatform_Alt = GameObject.Find ("Main Camera").GetComponent<DrawPlatform_Alt>();
		//All conditional on keyboard or GUI inputs. Implemented as small functions so GUI can directly call them.
		activateLevel(false);
		resetLevel(false);
		gotoMainMenu(false);
		toggleHelpText(false);
		changeColor(false);
		changePlatformType(false);
	}
	
	void OnLevelWasLoaded(){
		print ("New level loaded");
		helpText = GameObject.Find("help");
		isHelpTextEnabled = false;
		helpText.GetComponent<Canvas>().enabled = isHelpTextEnabled;
	}
}

