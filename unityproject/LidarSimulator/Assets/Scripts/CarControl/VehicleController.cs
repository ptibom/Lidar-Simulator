using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour {
	public float speed=80;
	float turnspeed=26;
	float force = 200;
	private float dir;
	private Rigidbody rb;

	//Use this for initialization
    void Start()
    {
        
    }

	void Update(){
        //rb = GetComponent<Rigidbody>();
        force = Input.GetAxis("Vertical");
        dir = Input.GetAxis("Horizontal");
    }
	void FixedUpdate(){
		// Move forward and backward 
		Vector3 movement = transform.forward * force * speed * Time.deltaTime;
        //rb.AddForce(transform.forward * force*speed);
        //rb.MovePosition (rb.position + movement);
        transform.Translate(movement);
        // Steering
        float turn = dir * turnspeed * Time.deltaTime;

		// Rotation in the y axis and apply to the rigidbody's rotation.
		Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);
        //rb.MoveRotation (rb.rotation * turnRotation);
        transform.rotation = transform.rotation * turnRotation;
	}


}
