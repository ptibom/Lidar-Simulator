using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Philip Tibom
/// Let's user control camera freely in the scene. Attach to camera component.
/// </summary>
public class FreeFlyingCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per physics frame
    void Update()
    {
        if (Input.GetButton("Fire1")) // Left mouse click. Do rotation.
        {
            float rotationAroundY = Input.GetAxis("Mouse X");
            float rotationAroundX = Input.GetAxis("Mouse Y");
            transform.Rotate(Vector3.up * rotationAroundY * Time.deltaTime * 400, Space.World);
            transform.Rotate(Vector3.right * -rotationAroundX * Time.deltaTime * 400, Space.Self);
        }
        if (Input.GetButton("Vertical")) // Move forwards or backwards
        {
            float forwardMovement = Input.GetAxis("Vertical");
            transform.position += transform.forward * forwardMovement * Time.deltaTime * 10;

        }
        if (Input.GetButton("Horizontal")) // Moving sideways.
        {
            float sidewaysMovement = Input.GetAxis("Horizontal");
            transform.position += transform.right * sidewaysMovement * Time.deltaTime * 10;
        }
    }
}
