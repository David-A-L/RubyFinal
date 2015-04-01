using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void SelectLevel(string scene_name){
		LevelManager.deleteAllPlatforms ();
		Application.LoadLevel (scene_name);
	}
}
