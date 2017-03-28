using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour {
	// Update is called once per frame
	public float setSpeed = 10f;

	void Update () {
		if(Input.GetKey(KeyCode.W)){
			transform.Translate (Vector3.forward * setSpeed * Time.deltaTime);
			transform.Rotate (0, 0, 0);		
		}

		if(Input.GetKey(KeyCode.A)){
			transform.Translate (Vector3.left * setSpeed * Time.deltaTime);
			transform.Rotate (0, -0.5f, 0);	
		}
		if(Input.GetKey(KeyCode.D)){
			transform.Translate (Vector3.right* setSpeed * Time.deltaTime);
			transform.Rotate (0, 0.5f, 0);	
		}

		if(Input.GetKey(KeyCode.S)){
			transform.Translate (Vector3.back* setSpeed * Time.deltaTime);
			transform.Rotate (0, 0, 0);
		}
	}
}
