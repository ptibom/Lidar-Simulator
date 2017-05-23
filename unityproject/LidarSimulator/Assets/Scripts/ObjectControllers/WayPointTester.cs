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
