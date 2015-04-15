using UnityEngine;
using System.Collections;

public class WHScript : MonoBehaviour {
	//TODO: MAKE THIS A TWO WAY PORTAL?
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
			Jukebox.Instance.playASound("WORMHOLE");
			collided.transform.position = exit.position;
		}
	}
}
