using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class MenuControllerScript : MonoBehaviour {

	LidarSensor sensor;
	GameObject mainCamera;
	GameObject lidarCamera;
	// Use this for initialization
	void Start () {
		sensor = GameObject.FindGameObjectWithTag ("Lidar").GetComponent<LidarSensor> ();
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		lidarCamera = GameObject.Find ("LidarCamera");


		//Set non used initial camera to inactive


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayButton(){
		//sensor.playSimulation = !sensor.playSimulation;
	}
		
	public void SwitchCamera(GameObject cameraGObject){

		mainCamera.SetActive(false);
		lidarCamera.SetActive (false);

		cameraGObject.SetActive (true);
	}
}
