using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour {
	public GameObject next;
	public GameObject previous;

	public static List<WayPoint> waypoints = new List<WayPoint>();
	// Use this for initialization
	void awake() {
		waypoints.Add (this);
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
