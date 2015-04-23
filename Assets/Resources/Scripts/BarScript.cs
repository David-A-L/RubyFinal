using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/*
 * script for modifying and bookkeeping of limiting bars
 */
public class BarScript : MonoBehaviour {

	public float baseMeterScale = 1;
	public Text displayText;

	// Use this for initialization
	void Start () {}

	void FixedUpdate(){}

	public void personalizeToPlayer(Player player,PlatformType pfrmType){
		switch (pfrmType) {
		case PlatformType.DEFAULT:
			gameObject.GetComponent<Image>().color = player.material.color;
			break;
		case PlatformType.CONVEYOR:
			//add trail renderer with particles matching player color
			gameObject.GetComponent<Image> ().color = Color.yellow;
			break;
		default:
			Debug.LogError ("Invalid bar type");
			break;
		}
	}

	public void setSize(float size){
		Vector3 temp = transform.localScale;
		temp.x = size;
		transform.localScale = temp;
		return;
	}

}
