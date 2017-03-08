using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRoller : MonoBehaviour {
	public float power = 10;

	Vector3 forceDirection = new Vector3(0,0,0);
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		forceDirection = new Vector3 (0, 0, 0);

		if (Input.GetKey (KeyCode.W)) {
			forceDirection = forceDirection + new Vector3 (0, 0, 1);
		}
		if(Input.GetKey (KeyCode.A)) {
			forceDirection = forceDirection + new Vector3 (-1, 0, 0);
		}
		if(Input.GetKey (KeyCode.S)) {
			forceDirection = forceDirection + new Vector3 (0, 0, -1);
		}
		if(Input.GetKey (KeyCode.D)) {
			forceDirection = forceDirection + new Vector3 (1, 0, 0);
		}
		forceDirection = forceDirection.normalized;
		GetComponent<Rigidbody> ().AddForce (forceDirection * power);
	}


}
