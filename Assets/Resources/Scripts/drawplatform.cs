using UnityEngine;
using System.Collections;

public class drawplatform : MonoBehaviour {

	public float difficulty = 5f; // Max 100;

	Vector3 lastPoint = Vector3.one;

	public Transform progressBar;
	public Material gray;
	public Material red;
	public Material blue;
	public Material yellow;
	public Material green;

	private string lastPfrmType;
	private GameObject pfrm;
	private Vector3 newPoint;
	private Vector3 newSize;
	private bool validLine = true;
	private bool first = true;
	// Use this for initialization
	void Start () {
		lastPfrmType = null;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Escape)){
			first = true;
			lastPoint = Vector3.one;
			newPoint = Vector3.zero;
			newSize = Vector3.zero;
			GameObject.Destroy(pfrm.gameObject);
			progressBar.GetComponent<BarScript>().revertToLocked();
		}
		bool inputFlag = false;
		if(Input.GetMouseButtonUp(0)) {
			lastPfrmType = "line";
			inputFlag = true;
		} else if(Input.GetKeyUp(KeyCode.C)) {
			lastPfrmType = "cnvrPfrm"; 
			inputFlag = true;
		}
		
		if(validLine && inputFlag){
			drawPlatform(lastPfrmType);
		}
		
		if(lastPoint.z != 1) transformLine(lastPfrmType);
	}

	void transformLine(string pfrmType) {

		// Endpoint
		newPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		newPoint.z = 0f;

		// Check if initial point exists
		if(lastPoint.z == 1) lastPoint = newPoint;

		// Put line center between endpoints
		pfrm.transform.position = (lastPoint + newPoint) / 2;

		// Get line length
		Vector3 difference = lastPoint - newPoint;

		// Get previous line length
		float oldSize = newSize.magnitude;

		// Create size for new line based on distance between endpoints
		newSize = Vector3.one * 0.25f;
		newSize.x = difference.magnitude;

		// Get change in lengths from last frame
		float deltaSize =  newSize.magnitude - oldSize;


		// Change the progress bar based on the the change in line size
		bool oldValidStatus = validLine;
		validLine = progressBar.GetComponent<BarScript>().changeSize(-1f * difficulty * deltaSize);

		// Set line color
		if(oldValidStatus != validLine) {
			if(validLine) {
				switch(pfrmType) {
				case "line":
					pfrm.GetComponent<Renderer>().GetComponent<Renderer>().material = gray;
					break;
				case "cnvrPfrm":
					pfrm.GetComponent<Renderer>().GetComponent<Renderer>().material = yellow;
					break;
				}
			}
			else {
				pfrm.GetComponent<Renderer>().GetComponent<Renderer>().material = red;
			}
		}
		// Update line object size and rotation
		pfrm.transform.localScale = newSize;	
		float angle = Mathf.Atan2(lastPoint.y - newPoint.y, lastPoint.x - newPoint.x) * Mathf.Rad2Deg;
		pfrm.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	void drawPlatform(string pfrmType)
	{
		newSize = Vector3.zero;
		if (!first) {
			pfrm.AddComponent<BoxCollider>();
			switch(pfrmType) {
			case "line":
				pfrm.GetComponent<Renderer>().GetComponent<Renderer>().material = blue;
				break;
			case "cnvrPfrm":
				pfrm.GetComponent<Renderer>().GetComponent<Renderer>().material = green;
				break;
			}
			
		}
		first = false;
		switch(pfrmType) {
			case "line":
			pfrm = (GameObject)Instantiate (Resources.Load ("Prefabs/line"));
			break;
			case "cnvrPfrm":
				pfrm = (GameObject)Instantiate (Resources.Load ("Prefabs/cnvrPfrm"));
			break;
		}
		transformLine(lastPfrmType);
		lastPoint = newPoint;
		progressBar.GetComponent<BarScript>().lockVal();
	}
}
