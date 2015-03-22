using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawPlatform_Alt : MonoBehaviour {
	
	public enum PlatformType{DEFAULT, CONVEYOR};
	enum DrawState{NONE, DRAWING};


	static Dictionary<PlatformType, GameObject> PlatPrfbDict;

	Dictionary<PlatformType, BarScript> PoolDict = new Dictionary<PlatformType, BarScript> ();
	Dictionary<PlatformType, bool> EnabledDict = new Dictionary<PlatformType, bool> ();

	//flags for enabling 
	public bool conveyorEnabled;
	
	PlatformType curPlatType = PlatformType.DEFAULT;
	DrawState curState = DrawState.NONE;

	public float difficulty = 5f; // Max 100;
	
	Vector3 lastPoint = Vector3.zero;

	GameObject pfrm;

	public Material red;
	public Transform progressBar;
/*	public Material gray;

	public Material blue;
	public Material yellow;
	public Material green;
*/
	private Vector3 newPoint;
	//private Vector3 newSize;
	private bool validLine = true;
	private bool first = true;

	/// <summary>
	/// Raises the runtime method load event.
	/// </summary>
	[RuntimeInitializeOnLoadMethod]
	static void OnRuntimeMethodLoad ()
	{
		Debug.Log("Filling Platform Dictionary");
		PlatPrfbDict = new Dictionary<PlatformType, GameObject>();
		PlatPrfbDict.Add (PlatformType.DEFAULT, (GameObject)Resources.Load ("Prefabs/line"));
		PlatPrfbDict.Add (PlatformType.CONVEYOR, (GameObject)Resources.Load("Prefabs/cnvrPfrm"));
	}

	// Use this for initialization
	void Start () {
		//Adding enabled and disabled for platform types for this level
		EnabledDict.Add (PlatformType.DEFAULT, true);
		EnabledDict.Add (PlatformType.CONVEYOR, conveyorEnabled);

		//TODO: gameObject.find() correct resource pool scripts on the gui and add to PoolDict

	}
	
	// Update is called once per frame
	void Update () {
		//TODO put this in the right place
		if (Input.GetKey (KeyCode.Backspace)) {
			Application.LoadLevel("_scene_main_menu");
		}

		if (Input.GetMouseButtonDown(0)) {
			curState = DrawState.DRAWING;
			lastPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			lastPoint.z = 0;
			pfrm = (GameObject)Instantiate (PlatPrfbDict [curPlatType]);
			progressBar.GetComponent<BarScript>().lockVal();
		}

		//Press C if enabled to toggle type
		//this could be generalized in a "Switch platform" function
		if (Input.GetKeyUp (KeyCode.C) && EnabledDict[PlatformType.CONVEYOR]
		    && curState != DrawState.DRAWING) {
			Debug.Log("Conveyor Toggle");
			if (curPlatType == PlatformType.DEFAULT){
				curPlatType = PlatformType.CONVEYOR;
			}
			else {
				curPlatType = PlatformType.DEFAULT;
			}

			//Could be possible to switch on the fly, disabled for now
			/*if (curState == DrawState.DRAWING){
				Vector3 tempP = pfrm.transform.position;
				Quaternion tempR = pfrm.transform.rotation;
				Vector3 tempS = pfrm.transform.localScale;

				pfrm = PlatPrfbDict[curPlatType];

				pfrm.transform.position = tempP;
				pfrm.transform.rotation = tempR;
				pfrm.transform.localScale = tempS;
			}*/
		}

		if (curState == DrawState.DRAWING) {
			//if right clicked (or x) while drawing a platform, cancel
			if (Input.GetMouseButton(2)||Input.GetKeyDown(KeyCode.X)) {
				//first = true;
				Debug.Log("Cancel Placement");
				lastPoint = Vector3.one;
				newPoint = Vector3.zero;
//				newSize = Vector3.zero;
				GameObject.Destroy (pfrm);
				progressBar.GetComponent<BarScript> ().revertToLocked ();
				curState = DrawState.NONE;
			}
			//else if released mouse button and valid platform
			else if (Input.GetMouseButtonUp (0) && validLine) {
				drawPlatform ();
			} 
			//else you are still modifying the line
			else {
				transformLine ();
			}

		}
	}
	
	void transformLine() {

		// Endpoint
		newPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		newPoint.z = 0f;

		
		// Put line center between endpoints
		pfrm.transform.position = (lastPoint + newPoint) / 2;

		Vector3 dir = newPoint - lastPoint;

		//find change in size
		float delta = dir.magnitude - pfrm.transform.localScale.x;

		//set pfrm scale and position
		Vector3 tempT = pfrm.transform.localScale;
		tempT.x = dir.magnitude;
		pfrm.transform.localScale = tempT;


		pfrm.transform.right = dir.normalized;

		validLine = progressBar.GetComponent<BarScript>().changeSize(difficulty * -delta);

		// Set line color
		if (!validLine) {
			pfrm.GetComponent<Renderer> ().material = red;
		}
		else {
			pfrm.GetComponent<Renderer> ().material =
				PlatPrfbDict[curPlatType].GetComponent<Renderer> ().sharedMaterial;
		}

	}
	
	void drawPlatform(){
		//TODO manage the newly created line
		pfrm.AddComponent<BoxCollider>();
		pfrm = null;
		curState = DrawState.NONE;
	}
}
