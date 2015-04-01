using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/*
 * script for modifying and bookkeeping of limiting bars
 */
public class BarScript : MonoBehaviour {
	public float lockedVal;
	public float baseVal = 100f;
	public float curVal;
	public float lastDelta;

	float xMax;
	float unitsPerPerc;
	public Text displayText;

	// Use this for initialization
	void Start () {
		xMax = transform.localScale.x;
		curVal = baseVal;
		unitsPerPerc = xMax / baseVal;
		lockedVal = baseVal;
	}

	void FixedUpdate(){
		//displayText.text = curVal.ToString();
	}

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

	public void lockVal() {
		lockedVal = curVal;
	}

	public void revertToLocked() {

		curVal = lockedVal;
		Vector3 temp = transform.localScale;
		temp.x = lockedVal * unitsPerPerc;
		transform.localScale = temp;
	}

	public bool changeSize(float delta) {

		curVal += delta;
		lastDelta = delta;

		if(curVal < 0) {
			//curVal = 0;
			Vector3 tmp = transform.localScale;
			tmp.x = 0f;
			transform.localScale = tmp;
			return false;
		}

		if (curVal > baseVal) {
			curVal = baseVal;
			return false;
		}

		Vector3 temp = transform.localScale;
		temp.x += delta * unitsPerPerc;
		transform.localScale = temp;
		return true;
	}

	/*
	 * reset bar to 100%
	 */
	public void ResetBar(){
		curVal = baseVal;
		Vector3 temp = transform.localScale;
		temp.x = xMax;
		transform.localScale = temp;
	}
	
	public float PercentLeft(){
		return curVal;
	}
}
