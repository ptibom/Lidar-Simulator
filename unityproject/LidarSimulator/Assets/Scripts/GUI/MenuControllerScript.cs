using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControllerScript : MonoBehaviour {


	LidarSensor sensor;

	// Use this for initialization
	void Start () {
		sensor = GameObject.FindGameObjectWithTag ("Lidar").GetComponent<LidarSensor> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void PlayButton(){
		sensor.playSimulation = !sensor.playSimulation;
	}
}
