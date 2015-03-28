using UnityEngine;
using System.Collections;

public class FallThroughPlatform : MonoBehaviour {
	private Color platformColor;
	private Collider platformColl;

	// Use this for initialization
	void Start () {
		platformColor = this.gameObject.GetComponent<Renderer>().material.color;
		platformColl = this.gameObject.GetComponent<Collider>();
	
	}
	
	void OnTriggerEnter(Collider coll) {
		if(coll.tag == "marble" && coll.gameObject.activeInHierarchy) {
			Color ballColor = coll.gameObject.GetComponent<Renderer>().material.color;
			if(ballColor != platformColor) {
				float yDisp = 2f*platformColl.bounds.extents.y;
				Vector3 prevPos = coll.gameObject.transform.position;
				prevPos.y -= yDisp;
				coll.gameObject.transform.position = prevPos;
			}
			
		}
	}
}
