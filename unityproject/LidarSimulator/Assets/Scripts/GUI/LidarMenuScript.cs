using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LidarMenuScript : MonoBehaviour {

	LidarSensor sensor;

	// Use this for initialization
	void Start () {
		sensor = GameObject.FindGameObjectWithTag ("Lidar").GetComponent<LidarSensor> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RowsOfLasers(Toggle toggle){
		if(toggle.isOn){
			sensor.rowsOfLasers = 2;
		} else {
			sensor.rowsOfLasers = 1;
		}
	}

	public void RotationSpeed(Slider slider){
		sensor.rotationSpeedHz = slider.value;
		slider.transform.FindChild ("Handle Slide Area").FindChild ("Handle").FindChild ("HandleText").GetComponent<Text> ().text = slider.value.ToString ();
	}

	public void LasersPerSet(Slider slider){
		sensor.rotationSpeedHz = slider.value;
		slider.transform.FindChild ("Handle Slide Area").FindChild ("Handle").FindChild ("HandleText").GetComponent<Text> ().text = slider.value.ToString ();
	}
}
