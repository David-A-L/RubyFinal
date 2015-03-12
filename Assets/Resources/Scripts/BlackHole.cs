using UnityEngine;
using System.Collections;

public class BlackHole : MonoBehaviour {

	public float Gconstant;
	public float maxForce;
	private GameObject ball;

	// Use this for initialization
	void Start () {
		ball = GameObject.Find("Marble");
	}
	
	Vector3 determineForce(GameObject ball){
		float ballMass = ball.GetComponent<Rigidbody>().mass;
		float bHoleMass = this.GetComponent<Rigidbody>().mass;
		
		Vector3 direction = ball.transform.position - this.transform.position;
		Debug.DrawRay(this.transform.position,direction,Color.red);
		
		float xForce = Gconstant * (ballMass * bHoleMass) / Mathf.Pow(direction.x,2f);
		float yForce = Gconstant * (ballMass * bHoleMass) / Mathf.Pow(direction.y,2f);
		xForce = Mathf.Min (xForce,maxForce*ballMass);
		yForce = Mathf.Min (yForce,maxForce*ballMass);
		
		if(this.transform.position.x < ball.transform.position.x) {
			xForce *= -1;
		} 
		if(this.transform.position.y < ball.transform.position.y) {
			yForce *= -1;
		} 
		
		return new Vector3(xForce,yForce,0f);
	}
	
	void applyForce(GameObject ball) {
		if(ball.GetComponent<Rigidbody>() != null){
			Vector3 force = determineForce(ball);
			print ("Appplied force "+force);
			ball.GetComponent<Rigidbody>().AddForce(force);
		}
	}
	
	// Update is called once per frame
	void Update () {
		applyForce(ball);
		if (Input.GetKey(KeyCode.O)) {
			if (maxForce > 0)
				maxForce -=1;
		}
		if (Input.GetKey(KeyCode.P)) {
			if (maxForce < 20)
				maxForce +=1;
		} 
	}
}
