using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class CC1 : MonoBehaviour
{
	public WheelCollider WFL;
	public WheelCollider WFR;
	public WheelCollider WBL;
	public WheelCollider WBR;
	public Transform FL;
	public Transform FR;
	public Transform BL;
	public Transform BR;
	private float speed;
	public float maxSpeed = 200f;
	public float maxWheelAngle = 45f;
	public float directionAngleMax = 40f;
	public float directionAngle = 20;
	public float accelerationCof = 10f;
	public float mTorque = 1000f;
	private bool isBraked = false;
	//public GameObject BrakeLight;


	void MoveMent()
	{
		speed = GetComponent<Rigidbody>().velocity.magnitude;

		WFL.brakeTorque = 0;
		WFR.brakeTorque = 0;
		WBL.brakeTorque = 0;
		WBR.brakeTorque = 0;
		WBR.motorTorque = Input.GetAxis("Vertical") * mTorque * accelerationCof * Time.deltaTime;
		WBL.motorTorque = Input.GetAxis("Vertical") * mTorque * accelerationCof * Time.deltaTime;
	}



	void rotateWheels()
	{
		FL.Rotate(WFL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
		FR.Rotate(WFR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
		BL.Rotate(WBL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
		BR.Rotate(WBR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
	}

	void Start()
	{
		GetComponent<Rigidbody>().centerOfMass = new Vector3(0.0f, -0.9f, 0.2f);
		//BrakeLight.SetActive(false);

	}

	void FixedUpdate()
	{
		speed = GetComponent<Rigidbody> ().velocity.magnitude;
		//move forward on UpArrow
		if ((Input.GetKey (KeyCode.UpArrow) && !isBraked) || ((Input.GetKey (KeyCode.DownArrow) && !isBraked))) {
			if (Input.GetKey (KeyCode.UpArrow)) {

				if (speed < maxSpeed) {
					MoveMent ();
					rotateWheels ();


				} else {
					WBR.motorTorque = 0;
					WBR.motorTorque = 0;

				}
			} else {

				WBR.motorTorque = 0;
				WBR.motorTorque = 0;
			}
		}

		//move backward on downArrow
		if ((Input.GetKey (KeyCode.UpArrow) && !isBraked) || ((Input.GetKey (KeyCode.DownArrow) && !isBraked))) {
			if (Input.GetKey (KeyCode.DownArrow)) {
				if (speed < maxSpeed) {
					MoveMent ();
					rotateWheels ();

				} else {
					WBR.motorTorque = 0;
					WBR.motorTorque = 0;

				}
			} else {
				WBR.motorTorque = 0;
				WBR.motorTorque = 0;
			}
		}

		//Vehicles steering

		directionAngle = (((maxWheelAngle - directionAngleMax) / maxSpeed) * speed) + directionAngleMax;
		WFL.steerAngle = Input.GetAxis ("Horizontal") * directionAngle;
		WFR.steerAngle = Input.GetAxis ("Horizontal") * directionAngle;
		rotateWheels ();

		//Brake

		if (Input.GetKey (KeyCode.Space)) {
			//BrakeLight.SetActive (true);
			isBraked = true;

			WBR.brakeTorque = Mathf.Infinity;
			WBL.brakeTorque = Mathf.Infinity;
			WFR.brakeTorque = Mathf.Infinity;
			WFL.brakeTorque = Mathf.Infinity;
			WBL.motorTorque = 0;
			WBR.motorTorque = 0;

		} else {
			isBraked = false;
			//BrakeLight.SetActive (false);
		}

	}
}
