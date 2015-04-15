using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	// Use this for initialization
	
	void Start () {
		if (Jukebox.Instance.firstMenuVisit) {
			Jukebox.Instance.playMainMenu ();
			Jukebox.Instance.firstMenuVisit = false;
		}


	}

	public void SelectLevel(string scene_name){
		if (Jukebox.Instance.firstLevelVisit) {
			Jukebox.Instance.playLevel ();;
			Jukebox.Instance.firstLevelVisit = false;
		}
		LevelManager.deleteAllPlatforms ();
		Application.LoadLevel (scene_name);
	}
}
