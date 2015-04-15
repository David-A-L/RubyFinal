using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public float rotPower;

	void FixedUpdate () {

		transform.Rotate (transform.forward * Time.deltaTime * rotPower);
		/*Vector3 tmp = transform.eulerAngles;
		tmp.z += rotPower;
		transform.eulerAngles += tmp;*/
	}
}
