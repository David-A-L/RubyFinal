using UnityEngine;
using System.Collections;

public class KeyCommandList : MonoBehaviour {

	public GameObject helpText;
	
	private static KeyCommandList instance;
	
	private bool enabled = false;
	private LevelManager levelManager;
	private DrawPlatform_Alt drawPlatform_Alt;
	
	public static KeyCommandList Instance { 
		get {
			if (instance == null){
				instance = new KeyCommandList();
			}
			return instance; 
		} 
	}
	//***Game Manager must call this to init this class***
	public void init(){
		helpText.SetActive (false);
	}
	public void changeColor(){
		if(Input.GetKeyUp(KeyCode.C) && Input.GetButton("shift")){
			drawPlatform_Alt.togglePlatforms(true);
		}
	}
	public void changePlatformType(){
		if(Input.GetKeyUp(KeyCode.C) && !Input.GetButton("shift")){
			drawPlatform_Alt.togglePlatforms(false);
		}
	}
	public void activateLevel(){
		if (Input.GetKeyUp(KeyCode.Space)){
			levelManager.ActivateLevel();
		}
	}
	public void resetLevel(){
		if (Input.GetKeyUp (KeyCode.R)) {
			levelManager.ResetLevel();
		}
	}
	public void gotoMainMenu(){
		if (Input.GetKey (KeyCode.Backspace)) {
			GameManager.Instance.disableAllBars();
			Application.LoadLevel("_scene_main_menu");
		}
	}
	public void toggleHelpText(){
		if(Input.GetKeyDown(KeyCode.H)) {
			print ("H");
			enabled = !enabled;
			helpText.SetActive (enabled);
		}
		if(Input.GetKeyDown(KeyCode.Escape)) {
			print ("Esc");
			if(enabled) {
				enabled = false;
				helpText.SetActive (false);
			}
		}
	}

	void Update() {
		levelManager = GameObject.Find ("Main Camera").GetComponent<LevelManager>();
		drawPlatform_Alt = GameObject.Find ("Main Camera").GetComponent<DrawPlatform_Alt>();
		
		//All conditional on keyboard or GUI inputs. Implemented as small functions so GUI can directly call them.
		activateLevel();
		resetLevel();
		gotoMainMenu();
		toggleHelpText();
		changeColor();
		changePlatformType();
	}
}

