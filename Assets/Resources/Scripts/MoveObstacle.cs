using UnityEngine;
using System.Collections;

public class MoveObstacle : MonoBehaviour {

	float speed;
	float frequencyFactor;
	int timeOffset;
	//TODO GENERALIZE THIS TO ANY AXIS OF MOTION (NOT JUST X)?
	// Use this for initialization
	void Start () {
		frequencyFactor = Random.value * 3f;
		timeOffset = Random.Range (0, 100);
		speed = (float)(Random.value - 0.5) * 3f; 
	}

	// Update is called once per frame
	void Update () {
		transform.Translate (new Vector3(Mathf.Sin (frequencyFactor * (Time.time + timeOffset)) * Time.deltaTime * speed, 0, 0));
	}
}
