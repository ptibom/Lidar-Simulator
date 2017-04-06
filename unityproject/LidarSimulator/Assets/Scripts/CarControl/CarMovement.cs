using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour {
	// Update is called once per frame
	public float moveSpeed = 10f;
    public float turnSpeed = 40f;
    void Update () {
		if(Input.GetKey(KeyCode.UpArrow))
			transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
		if(Input.GetKey(KeyCode.DownArrow ))
            transform.Translate (-Vector3.forward  * moveSpeed * Time.deltaTime);
		if(Input.GetKey(KeyCode.LeftArrow ))
			transform.Rotate  (Vector3.up , -turnSpeed * Time.deltaTime);
		if(Input.GetKey(KeyCode.RightArrow ))
			transform.Rotate  (Vector3.up ,turnSpeed * Time.deltaTime);
	}
}
