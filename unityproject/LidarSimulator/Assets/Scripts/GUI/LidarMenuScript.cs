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
	public Slider upperFOV;
	public Slider lowerFOV;
	public Slider offset;
	public Slider upperNormal;
	public Slider lowerNormal;
	public Slider simulationSpeed;

	private float[] simSpeedValues = {0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

	void Start () 
	{
		sensor = GameObject.FindGameObjectWithTag ("Lidar").GetComponent<LidarSensor> ();
		InitializeGUIValues ();
	}

	// A method which syncs the GUI lidar menu sliders with the initial values of the LidarSensor script
	void InitializeGUIValues()
	{
		// Number of lasers
		numberOfLasers.value = sensor.numberOfLasers;
		SetSliderText (numberOfLasers);
		// Rotationspeed
		rotationSpeedHz.value = sensor.rotationSpeedHz;
		SetSliderText(rotationSpeedHz);
		// Rotationangle
		rotationAnglePerStep.value = sensor.rotationAnglePerStep;
		SetSliderText (rotationAnglePerStep);
		// Ray distance
		rayDistance.value = sensor.rayDistance;
		SetSliderText (rayDistance);
		// Upper field of view
		upperFOV.value = sensor.upperFOV;
		SetSliderText (upperFOV);
		// lower field of view
		lowerFOV.value = sensor.lowerFOV;
		SetSliderText (lowerFOV);
		// Vertical offset between sets
		offset.value = sensor.offset;
		SetSliderText (offset);
		// Normal of top set (Positive upwards from horizontal axis)
		upperNormal.value = sensor.upperNormal;
		SetSliderText (upperNormal);
		// Normal of bottom set ((Positive downwards from horizontal axis)
		lowerNormal.value = sensor.lowerNormal;
		SetSliderText (lowerNormal);
		// Simulationspeed
		simulationSpeed.value = sensor.simulationSpeed;
		SetSliderText (simulationSpeed);
	}

	// Sets the text on the handle of the passed slider to the value of the slider
	public void SetSliderText(Slider slider)
	{
		slider.transform.FindChild ("Handle Slide Area").FindChild ("Handle").FindChild ("HandleText").GetComponent<Text> ().text = slider.value.ToString ();
	}

	// Sets the simulation speed in the LidarSensor script to the value in the GUI aswell as setting the handle text of the slider
	public void UpdateSimulationSpeed()
	{
		simulationSpeed.transform.FindChild ("Handle Slide Area").FindChild ("Handle").FindChild ("HandleText").GetComponent<Text> ().text = simSpeedValues[ (int)simulationSpeed.value ].ToString ();
		sensor.simulationSpeed = simSpeedValues[ (int)simulationSpeed.value ];
	}

	// A method which syncs the LidarSensor script with the settings in the GUI
	public void SendSettingsToLidar()
	{
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
