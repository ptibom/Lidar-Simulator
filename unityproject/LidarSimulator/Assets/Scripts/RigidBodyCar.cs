using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyCar : MonoBehaviour {
	public float speed;
	public float turnSpeed;
	public float extraGravity;
	public float reverseDecrease;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){


		Vector3 force = new Vector3 (0, 0, 0);
		Vector3 torque = new Vector3 (0, 0, 0);
		if (Input.GetAxis ("Vertical") > 0) {
			force = Vector3.Normalize (new Vector3 (transform.forward.x, 0, transform.forward.z));
		}
		else if(Input.GetAxis("Vertical") < 0) {
			force = -Vector3.Normalize (new Vector3 (transform.forward.x, 0, transform.forward.z))/reverseDecrease;
		}

		if (Input.GetAxis ("Horizontal") > 0) {
			torque = new Vector3(0,1,0);
		}
		else if(Input.GetAxis("Horizontal") < 0) {
			torque = -new Vector3(0,1,0);
		}

		GetComponent<Rigidbody> ().AddForce (Vector3.down*extraGravity);
		GetComponent<Rigidbody> ().AddForce (force*speed);
		GetComponent<Rigidbody> ().AddTorque (torque*turnSpeed);
	}
}
