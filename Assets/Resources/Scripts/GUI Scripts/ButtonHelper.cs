using UnityEngine;
using System.Collections;


/// <summary>
/// this class passes called button presses on to instantiated objects
/// </summary>
public class ButtonHelper : MonoBehaviour {

	public void CallReset(bool hardReset){
		Camera.main.GetComponent<LevelManager> ().ResetLevel (hardReset);
	}
}
