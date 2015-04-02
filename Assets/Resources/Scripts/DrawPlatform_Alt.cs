using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlatformType{DEFAULT, CONVEYOR};
public enum BuilderID {PLAYER1, PLAYER2, PLAYER3, PLAYER4};

public class DrawPlatform_Alt : MonoBehaviour {

	//HANDLES TO OTHER SCRIPTS
	private LevelManager levelManager;

	//ENUMS AND CLASSES FOR STATE INVOLVED WITH DRAWING
	public List <GameObject> allPossibleLines = new List<GameObject> ();
	public enum DrawState{NONE, DRAWING};
		
		//INSTANCES FOR ENUMS
		public static DrawState curState;
	
	//ENABLING FLAGS 
	public bool conveyorEnabled;

	//TWEAKING PARAMETERS
	public static float difficulty = 5f; // Max 100;
	public float thickness = .25f;

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

	void Awake(){}

	// Use this for initialization
	void Start () {
		curState = DrawState.NONE;
		levelManager = GameObject.Find ("Main Camera").GetComponent<LevelManager> ();

	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown(0)) {
			curState = DrawState.DRAWING;
			lastPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			lastPoint.z = 0; //bring to forefront

			pfrm = getPlatformFromType(levelManager.getCurrentPlayer().currentPlatformType);
			pfrm.transform.localScale = new Vector3 (0,thickness,1);

			levelManager.getCurrentPlayer().getCurrentPowerBarScript().lockVal();

			revalidation_material = pfrm.GetComponent<Renderer> ().material;
		}
		
		//Press C if enabled to toggle type
		if (Input.GetKeyUp (KeyCode.C) && curState != DrawState.DRAWING) {

			Player curPlayer = levelManager.getCurrentPlayer();
			PlatformType pt = levelManager.nextValidPlatformType(curPlayer.currentPlatformType);
			curPlayer.changeToNextPlatformType(pt);
			GameManager.Instance.disableAllBars ();
			curPlayer.getCurrentPowerBar ().SetActive (true);
//			if (pfrm != null){
//				CancelLine();
//				changePlatformBeingDrawnToNewType();
//			}
		}
		
		if (curState == DrawState.DRAWING) { 
			if (updateHelper_inputCancel()){return;}
			else if (updateHelper_mouseRelease ()){return;} 
			transformLine ();
		}
	}

//	private void changePlatformBeingDrawnToNewType(){
//		Player curPlayer = levelManager.getCurrentPlayer();
//		levelManager.getCurrentPlayer().getCurrentPowerBarScript().revertToLocked ();
//		GameObject temp = getPlatformFromType (curPlayer.currentPlatformType);
//
//		temp.transform.position = pfrm.transform.position;
//		temp.transform.localScale = pfrm.transform.localScale;
//		temp.transform.right = pfrm.transform.right;
//		revalidation_material = temp.GetComponent<Renderer> ().material;
//
//		GameObject.Destroy (pfrm);
//		pfrm = temp;
//		GameManager.Instance.disableAllBars ();
//		curPlayer.getCurrentPowerBar ().SetActive (true);
//	}

	GameObject getPlatformFromType (PlatformType pt){
		GameObject platformToBuild;
		switch (pt) {
			case PlatformType.CONVEYOR:
				platformToBuild = Instantiate (levelManager.getCurrentPlayer ().conveyorLine);
//				Material m = levelManager.getCurrentPlayer().particleMaterial;
//				platformToBuild.GetComponent<ConveyorPlatform>().SetParticleMaterial(m);
				break;
			case PlatformType.DEFAULT:
				platformToBuild = Instantiate (levelManager.getCurrentPlayer ().defaultLine);
				break;
			default:
				platformToBuild = null;
				Debug.LogError ("Can't draw platform, invalid platform type");
				break;
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

		//find change in size
		float delta = dir.magnitude - pfrm.transform.localScale.x;
		
		//set pfrm scale and position
		Vector3 tempT = pfrm.transform.localScale;
		tempT.x = dir.magnitude;

		pfrm.transform.localScale = tempT;
		pfrm.transform.right = dir.normalized;

		//bar script
		validLine = levelManager.getCurrentPlayer().getCurrentPowerBarScript().changeSize(difficulty * -delta);



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
		Debug.Log("Cancel Placement");

		//lastPoint = Vector3.one; //problem, forget lastPoint

		newPoint = Vector3.zero;
		GameObject.Destroy (pfrm);
		levelManager.getCurrentPlayer().getCurrentPowerBarScript().revertToLocked ();
		curState = DrawState.NONE;
	}

	void endTurn(){//end turn logic
		levelManager.tick ();
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


/*	
	GARBAGE (COMMENTED OUT CODE & VARs)

	public Material gray;
	public Transform progressBar;

	public Material blue;
	public Material yellow;
	public Material green;

	private Vector3 newSize;

	private bool first = true;
*/
