using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Jukebox.Instance.playMainMenu ();

	}

	public void SelectLevel(string scene_name){
		Jukebox.Instance.playLevel ();
		LevelManager.deleteAllPlatforms ();
		Application.LoadLevel (scene_name);
	}
}
