using UnityEngine;
using System.Collections;

public class MineHelper : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		if (other.tag == "marble") {
			transform.parent.GetComponent<MineScript>().Detonate();
		}
	}
}
