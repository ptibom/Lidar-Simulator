using System.Collections;
using System.Collections.Generic ;
using UnityEngine;
public class CC1 : MonoBehaviour 
{


	public WheelCollider WFL;
	public WheelCollider WFR;
	public WheelCollider WBL;
	public WheelCollider WBR;
	public Transform transformFL;
	public Transform transformFR;
	public Transform transformBL;
	public Transform transformBR;
	//public float speed= 50f ;
	public float mTorque = 50f;
	public float steer = 45F;
	public float brake =100f;


	private Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody> ();


	}

	void FixedUpdate () {
		WBR.brakeTorque = 0;
		WBL.brakeTorque =0;
		WBR.motorTorque = Input.GetAxis ("Vertical") * mTorque  ;
		WBL.motorTorque = Input.GetAxis ("Vertical") * mTorque ;
		WFL.steerAngle = Input.GetAxis ("Horizontal") * steer;
		WFL.steerAngle = Input.GetAxis ("Horizontal") * steer;
		if (Input.GetKey (KeyCode.Space)) {
			WBR.brakeTorque = brake;
			WBL.brakeTorque = brake;
		}
	}


}