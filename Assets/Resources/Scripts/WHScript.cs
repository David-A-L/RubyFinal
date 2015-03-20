using UnityEngine;
using System.Collections;

public class WHScript : MonoBehaviour {
	//Transform enter;
	Transform exit;
	// sets enter and exit points for the wormhole
	void Start () {
		//enter = transform.FindChild("Enter");
		exit = transform.FindChild ("Exit");
	}
	
	// called when a child is triggered
	public void EnterHandler(string child, Collider collided){
		if (child != "Enter")
			return;
		if (collided.tag == "marble") {
			collided.transform.position = exit.position;
		}
	}
}
