using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LidarMenuScript : MonoBehaviour {

	private LidarSensor sensor;

	public Slider numberOfLasers;
	public Slider rotationSpeedHz;
	public Slider rotationAnglePerStep;
	public Slider rayDistance;
	public Slider simulationSpeed;
	public Slider upperFOV;
	public Slider lowerFOV;
	public Slider offset;
	public Slider upperNormal;
	public Slider lowerNormal;


	// Use this for initialization
	void Start () {
		sensor = GameObject.FindGameObjectWithTag ("Lidar").GetComponent<LidarSensor> ();
		UpdateGUIValues ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateGUIValues(){
		numberOfLasers.value = sensor.numberOfLasers;
		SetSliderText (numberOfLasers);
		rotationSpeedHz.value = sensor.rotationSpeedHz;
		SetSliderText(rotationSpeedHz);
		rotationAnglePerStep.value = sensor.rotationAnglePerStep;
		SetSliderText (rotationAnglePerStep);

		//Skall fortsätta implementeras för alla parametrar
	}

	public void SetSliderText(Slider slider){
		slider.transform.FindChild ("Handle Slide Area").FindChild ("Handle").FindChild ("HandleText").GetComponent<Text> ().text = slider.value.ToString ();
	}

	public void RowsOfLasers(Toggle toggle){
		if(toggle.isOn){
		//	sensor.rowsOfLasers = 2;
		} else {
		//	sensor.rowsOfLasers = 1;
		}
	}
	/*
	public void RotationSpeed(Slider slider){
		//sensor.rotationSpeedHz = slider.value;
		slider.transform.FindChild ("Handle Slide Area").FindChild ("Handle").FindChild ("HandleText").GetComponent<Text> ().text = slider.value.ToString ();
	}
		
	public void LasersPerSet(Slider slider){
		sensor.rotationSpeedHz = slider.value;
		slider.transform.FindChild ("Handle Slide Area").FindChild ("Handle").FindChild ("HandleText").GetComponent<Text> ().text = slider.value.ToString ();
	}

	public void RotationAngle(Slider slider){
		sensor.rotationAnglePerStep = slider.value;
		slider.transform.FindChild ("Handle Slide Area").FindChild ("Handle").FindChild ("HandleText").GetComponent<Text> ().text = slider.value.ToString ();
	}

	public void RayDistance(Slider slider){
		sensor.rayDistance = slider.value;
		slider.transform.FindChild ("Handle Slide Area").FindChild ("Handle").FindChild ("HandleText").GetComponent<Text> ().text = slider.value.ToString ();

	}

	public void TopVerticalFOV(Slider slider){
		//sensor.verticalFOV = slider.value;
		slider.transform.FindChild ("Handle Slide Area").FindChild ("Handle").FindChild ("HandleText").GetComponent<Text> ().text = slider.value.ToString ();
	}

	public void BottomVerticalFOV(Slider slider){
		//sensor.verticalFOV = slider.value;
		slider.transform.FindChild ("Handle Slide Area").FindChild ("Handle").FindChild ("HandleText").GetComponent<Text> ().text = slider.value.ToString ();
	}

	public void RowOffset(Slider slider){
		sensor.offset = slider.value;
		slider.transform.FindChild ("Handle Slide Area").FindChild ("Handle").FindChild ("HandleText").GetComponent<Text> ().text = slider.value.ToString ();

	}

	public void SimulationSpeed(Slider slider){
		//sensor.simulationSpeed = slider.value;
		slider.transform.FindChild ("Handle Slide Area").FindChild ("Handle").FindChild ("HandleText").GetComponent<Text> ().text = slider.value.ToString ();

	}
	*/
}
