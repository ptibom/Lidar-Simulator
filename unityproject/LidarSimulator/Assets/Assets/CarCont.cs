using System.Collections;
using System.Collections.Generic ;
using UnityEngine;
public class CarCont : MonoBehaviour 
{
	public WheelCollider WFL;
	public WheelCollider WFR;
	public WheelCollider WBL;
	public WheelCollider WBR;
	public Transform FL;
	public Transform FR;
	public Transform BL;
	public Transform BR;
	public float speed;
	public float deacCoef =60;
	public float maxSpeed =200f;
	public float maxWheelAngle = 45f;
	public float directionAngleMax = 40f;
	public float directionAngle =20; 
	public float accelerationCof = 10f;
	public float mTorque = 1000f;
	public float brake =10000f;
	private bool isBraked = false;
	public GameObject BrakeLight;

	void Deaccelate()
	{
		//deaccelete 

			WBL.motorTorque = -2 * mTorque ;
			WBR.motorTorque = -2 * mTorque ;
			WBR.motorTorque = Input.GetAxis ("Vertical") * WBR.motorTorque  * deacCoef * Time.deltaTime;
			WBL.motorTorque = Input.GetAxis ("Vertical") * WBL.motorTorque  * deacCoef * Time.deltaTime;
	}


	void MoveMent()
	{
		WFL.brakeTorque = 0;
		WFR.brakeTorque = 0;
		WBL.brakeTorque = 0;
		WBR.brakeTorque = 0;
		WBR.motorTorque = Input.GetAxis ("Vertical") * mTorque * accelerationCof * Time.deltaTime;
		WBL.motorTorque = Input.GetAxis ("Vertical") * mTorque * accelerationCof * Time.deltaTime;
	}

		

	void rotateWheels()
	{
		FL.Rotate (WFL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
		FR.Rotate (WFR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
		BL.Rotate (WBL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
		BR.Rotate (WBR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
	}

	void Start () 
	{
		GetComponent<Rigidbody> ().centerOfMass = new Vector3 (0.0f, -0.9f, 0.2f);
		BrakeLight.SetActive (false);

	}

	void FixedUpdate ()
	{
		speed = GetComponent<Rigidbody> ().velocity.magnitude;
		//move forward on UpArrow
		if ((Input.GetKey (KeyCode.UpArrow)) && !isBraked) {
			if (speed < maxSpeed) {
				MoveMent ();
				rotateWheels ();

			} else {
				Deaccelate ();

			}
		}
		//move backward on downArrow
		if ((Input.GetKey (KeyCode.DownArrow)) && !isBraked) {
			if (speed < maxSpeed) {
				MoveMent ();
				rotateWheels ();

			} 
			else 
			{
				Deaccelate ();

			}
		}
	
		//Vehicles steering

		directionAngle = (((maxWheelAngle - directionAngleMax) / maxSpeed) * speed) + directionAngleMax;
		WFL.steerAngle = Input.GetAxis ("Horizontal") * directionAngle;
		WFR.steerAngle = Input.GetAxis ("Horizontal") * directionAngle;
		rotateWheels ();
	
		//Brake

		if (Input.GetKey (KeyCode.Space)) {
			BrakeLight.SetActive (true);
			isBraked = true;

			WBR.brakeTorque = Mathf.Infinity;
			WBL.brakeTorque = Mathf.Infinity;
			WFR.brakeTorque = Mathf.Infinity;
			WFL.brakeTorque = Mathf.Infinity;
			WBL.motorTorque = 0;
			WBR.motorTorque = 0;
		
		} else {
			isBraked = false;
			BrakeLight.SetActive (false);
		}
	}
}
