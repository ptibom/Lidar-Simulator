/*
* MIT License
* 
* Copyright (c) 2017 Philip Tibom, Jonathan Jansson, Rickard Laurenius, 
* Tobias Alldén, Martin Chemander, Sherry Davar
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Philip Tibom
/// Let's user control camera freely in the scene. Attach to camera component.
/// </summary>
public class FreeFlyingCamera : MonoBehaviour {

    public int flyingSpeed = 50;

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
            transform.position += transform.forward * forwardMovement * Time.deltaTime * flyingSpeed;

        }
        if (Input.GetButton("Horizontal")) // Moving sideways.
        {
            float sidewaysMovement = Input.GetAxis("Horizontal");
            transform.position += transform.right * sidewaysMovement * Time.deltaTime * flyingSpeed;
        }
    }
}
