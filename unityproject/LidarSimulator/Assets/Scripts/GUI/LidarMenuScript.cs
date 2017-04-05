using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LidarMenuScript : MonoBehaviour {

	public LidarSensor sensor;
	public Slider numberOfLasers;
	public Slider rotationSpeedHz;
	public Slider rotationAnglePerStep;
	public Slider rayDistance;
	public Slider upperFOV;
	public Slider lowerFOV;
	public Slider offset;
	public Slider upperNormal;
	public Slider lowerNormal;
	public Slider simulationSpeed;

	private float[] simSpeedSliderValues = {0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

	void Start () 
	{
		InitializeGUIValues ();
	}

	// A method which syncs the GUI lidar menu sliders with the initial values of the LidarSensor script
	void InitializeGUIValues()
	{
		// Number of lasers
		numberOfLasers.value = sensor.numberOfLasers;
		// Rotationspeed
		rotationSpeedHz.value = sensor.rotationSpeedHz;
		// Rotationangle
		rotationAnglePerStep.value = sensor.rotationAnglePerStep;
		// Ray distance
		rayDistance.value = sensor.rayDistance;
		// Upper field of view
		upperFOV.value = sensor.upperFOV;
		// lower field of view
		lowerFOV.value = sensor.lowerFOV;
		// Vertical offset between sets
		offset.value = sensor.offset;
		// Normal of top set (Positive upwards from horizontal axis)
		upperNormal.value = sensor.upperNormal;
		// Normal of bottom set ((Positive downwards from horizontal axis)
		lowerNormal.value = sensor.lowerNormal;

		// Simulationspeed
		if (sensor.simulationSpeed < 1) {
			simulationSpeed.value = (int)(sensor.simulationSpeed * 10 - 1);
			UpdateSimulationSpeed();
		} else {
			simulationSpeed.value = (int)(sensor.simulationSpeed + 8);
			UpdateSimulationSpeed();
		}

	}
		
	// Sets the simulation speed in the LidarSensor script to the value in the GUI aswell as setting the handle text of the slider
	public void UpdateSimulationSpeed()
	{
		sensor.simulationSpeed = simSpeedSliderValues [(int)simulationSpeed.value];
		UpdateSliderHandleText (simulationSpeed, simSpeedSliderValues [(int)simulationSpeed.value]);

	}

	//Update slider handle text
	public void UpdateSliderHandleText(Slider slider, float newVal){
		slider.transform.FindChild ("Handle Slide Area").FindChild ("Handle").FindChild ("HandleText").GetComponent<Text> ().text = newVal.ToString ();
	}

	// A method which syncs the slider values to the visual values in the inputField boxes
	public void SyncInputFieldToSlider(){
		numberOfLasers.GetComponent<InputSyncScript> ().SyncToSlider ();
		rotationSpeedHz.GetComponent<InputSyncScript> ().SyncToSlider ();
		rotationAnglePerStep.GetComponent<InputSyncScript> ().SyncToSlider ();
		rayDistance.GetComponent<InputSyncScript> ().SyncToSlider ();
		upperFOV.GetComponent<InputSyncScript> ().SyncToSlider ();
		lowerFOV.GetComponent<InputSyncScript> ().SyncToSlider ();
		offset.GetComponent<InputSyncScript> ().SyncToSlider ();
		upperNormal.GetComponent<InputSyncScript> ().SyncToSlider ();
		lowerNormal.GetComponent<InputSyncScript> ().SyncToSlider ();
	}

	// A method which syncs the LidarSensor script with the settings in the GUI
	public void SendSettingsToLidar()
	{
		SyncInputFieldToSlider ();

		sensor.numberOfLasers = (int)numberOfLasers.value;
		sensor.rotationSpeedHz = rotationSpeedHz.value;
		sensor.rotationAnglePerStep = rotationAnglePerStep.value;
		sensor.rayDistance = rayDistance.value;
		sensor.upperFOV = upperFOV.value;
		sensor.lowerFOV = lowerFOV.value;
		sensor.offset = offset.value;
		sensor.upperNormal = upperNormal.value;
		sensor.lowerNormal = lowerNormal.value;
	}


}
