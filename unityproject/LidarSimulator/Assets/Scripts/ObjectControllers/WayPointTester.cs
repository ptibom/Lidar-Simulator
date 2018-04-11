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

public class WayPointTester : MonoBehaviour {
	public GameObject testThisPath;
	float time = 0f;
	public bool deletePath = false;
	public bool testvisibility = false;
	public bool testColliders = false;
	private bool changedToInvisible = false;
	private bool changedToVisible = false;
	private bool disabledColliders = false;
	private bool enabledColliders = false;
	// Use this for initialization
	void Start () {
		Debug.Log ("WayPointTester in scene");
	}
	
	// Update is called once per frame
	void Update () {
		time = time + Time.deltaTime;
		if (time > 3f && deletePath == true) {
			deletePath = false;
			Debug.Log ("Calling removePath()");
            testThisPath.GetComponent<WayPoint>().RemovePath();
			//WayPoint.RemovePath (testThisPath);
			//WayPoint.TestMethod(testThisPath);
		}
		if (time > 3f && testvisibility == true && changedToInvisible == false) {
			deletePath = false;
			Debug.Log ("Invisible");
			//testThisPath.GetComponent<WayPoint> ().SetVisibility(false);
			WayPoint.SetGlobalVisibility(false);
			changedToInvisible = true;
			//WayPoint.TestMethod(testThisPath);
		}
		if (time > 6f && testvisibility == true && changedToVisible == false) {
			deletePath = false;
			Debug.Log ("Visible");
			changedToVisible = true;
			WayPoint.SetGlobalVisibility (true);
			//testThisPath.GetComponent<WayPoint>().SetVisibility(true);
			//WayPoint.TestMethod(testThisPath);
		}


		if (time > 3f && testColliders == true && enabledColliders == false) {
			deletePath = false;
			Debug.Log ("Colliders on");
			//testThisPath.GetComponent<WayPoint> ().SetVisibility(false);
			WayPoint.SetGlobalColliderState(true);
			enabledColliders = true;
			//WayPoint.TestMethod(testThisPath);
		}
		if (time > 6f && testColliders == true && disabledColliders == false) {
			deletePath = false;
			Debug.Log ("Colliders off");
			disabledColliders = true;
			WayPoint.SetGlobalColliderState(false);
			//testThisPath.GetComponent<WayPoint>().SetVisibility(true);
			//WayPoint.TestMethod(testThisPath);
		}
	}
}
