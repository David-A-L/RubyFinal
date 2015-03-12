using UnityEngine;
using System.Collections;

public class drawplatform : MonoBehaviour {

	public float difficulty = 5f; // Max 100;

	Vector3 lastPoint = Vector3.one;

	public Transform progressBar;
	public Transform line;
	public Material gray;
	public Material red;
	public Material blue;

	private Transform cube;
	private Vector3 newPoint;
	private Vector3 newSize;
	private bool validLine = true;
	private bool first = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Escape)){
			first = true;
			lastPoint = Vector3.one;
			newPoint = Vector3.zero;
			newSize = Vector3.zero;
			GameObject.Destroy(cube.gameObject);
			progressBar.GetComponent<BarScript>().revertToLocked();
		}
		if(Input.GetMouseButtonUp(0)) {
			if(validLine) {
				drawPlatform();
			}
		}
		if(lastPoint.z != 1) transformLine();
	}

	void transformLine() {

		// Endpoint
		newPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		newPoint.z = 0f;

		// Check if initial point exists
		if(lastPoint.z == 1) lastPoint = newPoint;

		// Put line center between endpoints
		cube.transform.position = (lastPoint + newPoint) / 2;

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
				cube.GetComponent<Renderer>().GetComponent<Renderer>().material = gray;
			}
			else {
				cube.GetComponent<Renderer>().GetComponent<Renderer>().material = red;
			}
		}
		// Update line object size and rotation
		cube.transform.localScale = newSize;	
		float angle = Mathf.Atan2(lastPoint.y - newPoint.y, lastPoint.x - newPoint.x) * Mathf.Rad2Deg;
		cube.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	void drawPlatform()
	{
		newSize = Vector3.zero;
		if (!first) {
			cube.gameObject.AddComponent<BoxCollider>();
			cube.GetComponent<Renderer>().GetComponent<Renderer>().material = blue;
		}
		first = false;
		cube = Instantiate(line);
		transformLine();
		lastPoint = newPoint;
		progressBar.GetComponent<BarScript>().lockVal();
	}
}
