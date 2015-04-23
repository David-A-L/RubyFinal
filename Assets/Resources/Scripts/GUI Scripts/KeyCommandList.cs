using UnityEngine;
using System.Collections;

public class KeyCommandList : MonoBehaviour {
	public GameObject helpText;
	private bool enabled = false;
	void Start() {
		helpText.SetActive (false);
	}
	
	void Update() {
		
		/*----------ACTIVATE LEVEL----------*/
		if (Input.GetKeyUp(KeyCode.Space)){
			LevelManger.ActivateLevel();
		}
		/*----------RESET LEVEL----------*/
		/*----------GOTO MAIN MENU----------*/
		/*----------HELP TEXT----------*/
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
		/*----------CHANGE COLOR----------*/
		/*----------CHANGE PLATFORM TYPE----------*/
		
	}
}

