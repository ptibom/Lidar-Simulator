using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointTester : MonoBehaviour {
	public GameObject testThisPath;
	float time = 0f;
	public bool deletePath = false;
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
			WayPoint.removePath (testThisPath);
			//WayPoint.TestMethod(testThisPath);
		}
	}
}
