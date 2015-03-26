using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawPlatform_Alt : MonoBehaviour {
	
	private LevelManager levelManager;
	public enum PlatformType{DEFAULT, CONVEYOR};
	public enum DrawState{NONE, DRAWING};

	public float thickness = .25f;

	static Dictionary<PlatformType, GameObject> PlatPrfbDict;
	
	public static Dictionary<PlatformType, BarScript> PoolDict = new Dictionary<PlatformType, BarScript> ();
	Dictionary<PlatformType, bool> EnabledDict = new Dictionary<PlatformType, bool> ();
	
	//flags for enabling 
	public bool conveyorEnabled;
	
	PlatformType curPlatType = PlatformType.DEFAULT;
	public static DrawState curState;
	
	public static float difficulty = 5f; // Max 100;
	
	Vector3 lastPoint = Vector3.zero;
	
	GameObject pfrm;
	
	public Material red;
	//public Transform progressBar;
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
		PoolDict = new Dictionary<PlatformType, BarScript> ();
		Debug.Log("Filling Platform Dictionary");
		PlatPrfbDict = new Dictionary<PlatformType, GameObject>();
		PlatPrfbDict.Add (PlatformType.DEFAULT, (GameObject)Resources.Load ("Prefabs/line"));
		PlatPrfbDict.Add (PlatformType.CONVEYOR, (GameObject)Resources.Load("Prefabs/cnvrPfrm"));
	}

	void Awake(){
		PoolDict = new Dictionary<PlatformType, BarScript> ();
	}

	// Use this for initialization
	void Start () {
		//Adding enabled and disabled for platform types for this level
		curState = DrawState.NONE;
		levelManager = GameObject.Find ("Main Camera").GetComponent<LevelManager> ();
		EnabledDict.Add (PlatformType.DEFAULT, true);
		EnabledDict.Add (PlatformType.CONVEYOR, conveyorEnabled);

		PoolDict = new Dictionary<PlatformType, BarScript> ();
		PoolDict.Add (PlatformType.DEFAULT, GameObject.FindGameObjectWithTag ("pool_default").GetComponent<BarScript> ());
		if (conveyorEnabled)
			PoolDict.Add (PlatformType.CONVEYOR, GameObject.FindGameObjectWithTag ("pool_conveyor").GetComponent<BarScript> ());
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown(0)) {
			curState = DrawState.DRAWING;
			lastPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			lastPoint.z = 0;
			pfrm = (GameObject)Instantiate (PlatPrfbDict [curPlatType]);
			pfrm.transform.localScale = new Vector3 (0,thickness,1);
			PoolDict[curPlatType].lockVal();
		}
		
		//Press C if enabled to toggle type
		//this could be generalized in a "Switch platform" function
		if (Input.GetKeyUp (KeyCode.C)) {
			TogglePlatformType(PlatformType.CONVEYOR);
		}
		
		if (curState == DrawState.DRAWING) {
			//if right clicked (or x) while drawing a platform, cancel
			if (Input.GetMouseButton(2)||Input.GetKeyDown(KeyCode.X)) {
				CancelLine();
			}
			//else if released mouse button and valid platform
			else if (Input.GetMouseButtonUp (0)) {
				if (validLine)
					drawPlatform ();
				else 
					CancelLine();
			} 
			//else you are still modifying the line
			else {
				transformLine ();
			}
			
		}
	}

	void TogglePlatformType(PlatformType pt){
		if (EnabledDict [pt] && curState != DrawState.DRAWING) {
			if (curPlatType != pt) {
				curPlatType = pt;
			} else {
				curPlatType = PlatformType.DEFAULT;
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
		
		validLine = PoolDict[curPlatType].changeSize(difficulty * -delta);
		
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
		pfrm.AddComponent<BoxCollider>(); 
		levelManager.AddPlatform (pfrm, curPlatType);
		pfrm = null;
		curState = DrawState.NONE;
	}

	void CancelLine(){
		Debug.Log("Cancel Placement");
		lastPoint = Vector3.one;
		newPoint = Vector3.zero;
		GameObject.Destroy (pfrm);
		PoolDict[curPlatType].revertToLocked ();
		curState = DrawState.NONE;
	}
}
