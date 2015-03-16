using UnityEngine;
using System.Collections;

public class RotateScene : MonoBehaviour {
	
	public float rotateSpeed;
	public bool autoRotate;
	public GameObject center;
	public GameObject marble;
	public float artifGravity;
	
	
	void Start() {
		if(rotateSpeed != 0) {
			Physics.gravity = Vector3.zero;
		} else {
			Physics.gravity = new Vector3(0f,-9.8f,0f);
		}
	}

	// Update is called once per frame
	void Update () {
		if(rotateSpeed != 0) {
			float amtRotate = Time.deltaTime * rotateSpeed;
			if(!autoRotate) {
				if(Input.GetKey(KeyCode.LeftArrow)){
					amtRotate *= -1f;
				} else if (!Input.GetKey(KeyCode.RightArrow)) {
					amtRotate = 0;
				}
			}
			
			Camera.main.transform.RotateAround(Camera.main.transform.position,transform.forward,amtRotate);
			ApplyGravity();
		}
	}
	
	void ApplyGravity() {
		Rigidbody mRigid = marble.gameObject.GetComponent<Rigidbody>();
		if(mRigid != null) {
			Vector3 forcePos = center.transform.position - marble.transform.position;
			mRigid.AddForce((forcePos).normalized * artifGravity * Time.smoothDeltaTime);
		}
	}
}
