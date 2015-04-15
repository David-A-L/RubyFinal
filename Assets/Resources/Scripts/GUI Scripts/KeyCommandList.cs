using UnityEngine;
using System.Collections;

public class KeyCommandList : MonoBehaviour {
	public GameObject helpText;
	private bool enabled = false;
	void Start() {
		helpText.SetActive (false);
	}

	void Update() {
		print ("running");
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
}

