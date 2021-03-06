﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlatformType{DEFAULT, CONVEYOR};
public enum BuilderID {PLAYER1, PLAYER2, PLAYER3, PLAYER4};

public class DrawPlatform_Alt : MonoBehaviour {


	public static float rotateSpeed = .005f;

	//HANDLES TO OTHER SCRIPTS
	private LevelManager levelManager;

	//ENUMS AND CLASSES FOR STATE INVOLVED WITH DRAWING
	public enum DrawState{NONE, DRAWING, MOVING};
		
	//INSTANCES FOR ENUMS
	public static DrawState curState;
	
	//ENABLING FLAGS 
	public bool conveyorEnabled;

	//TWEAKING PARAMETERS
	public static float difficulty = 5f; // Max 100;
	public float thickness = .25f;
	public static float minPlatLen = .5f;

	//MATERIALS
	public Material red;
	private Material revalidation_material;

	
	//UPDATE VARIABLES (CONSTANTLY USED/CHANGED IN PROCESS OF DRAWING)
		
		//GAME OBJ FOR LINE
		GameObject pfrm;

		//ENDPOINTS
		private Vector3 newPoint;
		Vector3 lastPoint = Vector3.zero;

		//BOOLS
		private bool validLine = true;

	//GAME OBJECT MODIFICATION
	private Transform selectedTrans = null;

	void Awake(){}

	// Use this for initialization
	void Start () {
		curState = DrawState.NONE;
		levelManager = GameObject.Find ("Main Camera").GetComponent<LevelManager> ();
		levelManager.EnabledDict [PlatformType.CONVEYOR] = conveyorEnabled;
	}
	
	// Update is called once per frame
	void Update () {

		if (LevelManager.curLevelState != LevelManager.LevelState.RUNNING) {

			if (Input.GetMouseButtonDown (0)) {
				//Check if you clicked on an object
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				ray.direction *= 1000f;

				if (Physics.Raycast (ray, out hit) && hit.transform.tag == "movable") {
					if (curState == DrawState.MOVING){
						selectedTrans = null;
						curState = DrawState.NONE;
					}
					else {
						Debug.Log ("Now in move mode");
						selectedTrans = hit.transform;
						Debug.Log ("Moving " + hit.collider.name);
						curState = DrawState.MOVING;
					}
				}
				//Else, drawing a new line
				else {
						curState = DrawState.DRAWING;
						lastPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						lastPoint.z = 0; //bring to forefront

						pfrm = getPlatformFromType (levelManager.getCurrentPlayer ().currentPlatformType);
						pfrm.transform.localScale = new Vector3 (0, thickness, 1);

						revalidation_material = pfrm.GetComponent<Renderer> ().material;
					}
			}
			
			//Toggling platforms handled by KeyCommandList
		
			if (curState == DrawState.DRAWING) { 
				if (updateHelper_inputCancel ()) {
					return;
				} else if (updateHelper_mouseRelease ()) {
					return;
				} 
				transformLine ();
			}

			//if you are moving a line you want to delete
			if (curState == DrawState.MOVING && Input.GetKeyUp(KeyCode.Delete)){
				//HACK...not very pretty
				if(selectedTrans.name.Contains("line")){
					//try catch just in case...
					try {
						levelManager.DeletePlatform(selectedTrans.gameObject);
					} catch (UnityException ex){
						Debug.Log("Unable to delete platform");
						Debug.Log (ex.ToString());
						return;
					}

					selectedTrans = null;
					curState = DrawState.NONE;
				}
			}

		}

	}
	
	public void togglePlatforms(bool shiftHeld){
		Player curPlayer = levelManager.getCurrentPlayer ();
		PlatformType pt = curPlayer.currentPlatformType;
		
		//Switch color
		if (shiftHeld){
			levelManager.tick();
			curPlayer = levelManager.getCurrentPlayer ();//update this since changed
		}
		//Switch platform type
		else{
			pt = levelManager.nextValidPlatformType (curPlayer.currentPlatformType);
		}
		curPlayer.changeToPlatformType (pt);
			
		if (curState == DrawState.DRAWING) {
			Vector3 tempLocalScale = pfrm.transform.localScale;
			GameObject.Destroy(pfrm);
			pfrm = getPlatformFromType(pt);
			pfrm.transform.localScale = tempLocalScale;
			revalidation_material = pfrm.GetComponent<Renderer>().material;
		}
		
		levelManager.refreshShowingBar ();
	}

	//MOVE CONTROLS
	void FixedUpdate(){
		//Bit of a HACK, probably could be handled more cleanly
		//could add more functionality like rotate, delete, etc...
		if (curState == DrawState.MOVING) {
			Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mouse.z = 0;
			selectedTrans.position = mouse;
			float rotDir = Input.GetAxis("Mouse ScrollWheel");

			/*Debug.Log("before:" + selectedTrans.up);
			Vector3.RotateTowards(selectedTrans.up,selectedTrans.right,0.261799f * rotDir,1f);
			Debug.Log("After:" + selectedTrans.up);*/
			Vector3 curRot = selectedTrans.eulerAngles;
			curRot.z +=  rotDir * rotateSpeed;
			selectedTrans.eulerAngles = curRot;
		}
	}

	GameObject getPlatformFromType (PlatformType pt){
		GameObject platformToBuild;
		switch (pt) {
			//HACK this is uuuuugly
			case PlatformType.CONVEYOR:
				platformToBuild = Instantiate (levelManager.getCurrentPlayer ().conveyorLine);
				break;
			case PlatformType.DEFAULT:
				platformToBuild = Instantiate (levelManager.getCurrentPlayer ().defaultLine);
				break;
			default:
				platformToBuild = null;
				Debug.LogError ("Can't draw platform, invalid platform type");
				break;
			}
		if (platformToBuild != null) {
			platformToBuild.transform.position = new Vector3 (0, 0, -100);
		}
		return platformToBuild;
	}
	
	void transformLine() {
		
		// Endpoint
		newPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		newPoint.z = 0f;
		
		// Put line center between endpoints
		pfrm.transform.position = (lastPoint + newPoint) / 2;
		Vector3 dir = newPoint - lastPoint;

		//set pfrm scale and position
		Vector3 tempT = pfrm.transform.localScale;
		tempT.x = dir.magnitude;

		pfrm.transform.localScale = tempT;
		pfrm.transform.right = dir.normalized;

		//bar script
		float curBarSize = levelManager.calculateBarSizeForCurrentEverything_plus_PlatformP (pfrm);
		validLine = (curBarSize >= 0 && pfrm.transform.localScale.x >= minPlatLen);
		if (!validLine) {
			curBarSize = 0f;
		}

		levelManager.getCurrentPlayer ().getCurrentPowerBarScript ().setSize (curBarSize);

		// Set line color
		if (!validLine) {pfrm.GetComponent<Renderer>().material = red;}
		else{pfrm.GetComponent<Renderer>().material = revalidation_material;}

	}
	
	void drawPlatform(){
		pfrm.AddComponent<BoxCollider>();
		levelManager.AddPlatform (pfrm);
		endTurn ();
	}

	void CancelLine(){
		newPoint = Vector3.zero;
		GameObject.Destroy (pfrm);
		curState = DrawState.NONE;
		levelManager.updateShowingBar ();
	}

	public void endTurn(){//end turn logic
		pfrm = null;
		curState = DrawState.NONE;
	}
	

//HELPERS FOR INPUT: SIMPLIFY UPDATE LOGIC
	
	//THESE BOOLS RETURN TRUE IF USER DID PERFORM SPECIFIED INPUT (the fn name), THUS CALLER KNOWS TO RETURN IMMEDIATELY
	bool updateHelper_mouseRelease (){	//WHEN USER RELEASES MOUSE DRAWING
		if (Input.GetMouseButtonUp (0)) {
			if (validLine){drawPlatform();}
			else{CancelLine();}
			return true;
		}
		return false;
	}
	bool updateHelper_inputCancel (){//IF (R CLK, OR X) CANCEL
		if (Input.GetMouseButton(2)||Input.GetKeyDown(KeyCode.X)) {CancelLine(); return true;}
		return false;
	}
}
