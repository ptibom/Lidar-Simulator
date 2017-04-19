using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LidarMenuScript : MonoBehaviour {

	public static event UpdateMimicValues OnLidarMenuValChanged;
    public static event PassLidarMenuValues PassGuiValsOnStart;
    public delegate void UpdateMimicValues(int noLasers, float upFOV, float lowFOV, float offset, float upNorm, float lowNorm);
    public delegate void PassLidarMenuValues(int numberOfLasers, float rotationSpeed, float rotationAnglePerStep, float rayDistance, float upperFOV
        , float lowerFOV, float offset, float upperNormal, float lowerNormal);

	[SerializeField]
	private Slider numberOfLasers;
	[SerializeField]
	private Slider rotationSpeedHz;
	[SerializeField]
	private Slider rotationAnglePerStep;
	[SerializeField]
	private Slider rayDistance;
	[SerializeField]
	private Slider upperFOV;
	[SerializeField]
	private Slider lowerFOV;
	[SerializeField]
	private Slider offset;
	[SerializeField]
	private Slider upperNormal;
	[SerializeField]
	private Slider lowerNormal;
	[SerializeField]
	private Slider simulationSpeed;

    [SerializeField]
    private TimeManager timeManager;
    [SerializeField]
    private LidarLineMimic lidarLineMimic;

    private LidarSensor sensor;

    private float[] simSpeedSliderValues = {0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

	void Start () 
	{
        sensor = GameObject.FindGameObjectWithTag("Lidar").GetComponent<LidarSensor>();
		lidarLineMimic.InitializeLaserMimicList ();
		InitializeGUIValues ();
		UpdateLaserMimicValues ();
	}
		
	// A method which syncs the GUI lidar menu sliders with the initial values of the LidarSensor and TimeManager script 
	void InitializeGUIValues()
	{
        Debug.Log("noL:");
		numberOfLasers.value = sensor.numberOfLasers;
        Debug.Log("rotSpeed");
		rotationSpeedHz.value = sensor.rotationSpeedHz;
        Debug.Log("rotAngl");
        rotationAnglePerStep.value = sensor.rotationAnglePerStep;
        Debug.Log("dist");
        rayDistance.value = sensor.rayDistance;
        Debug.Log("uFov");
        upperFOV.value = sensor.upperFOV;
        Debug.Log("lFov");
        lowerFOV.value = sensor.lowerFOV;
        Debug.Log("offs");
        offset.value = sensor.offset;
        Debug.Log("uNorm");
        upperNormal.value = sensor.upperNormal;
        Debug.Log("lNorm");
        lowerNormal.value = sensor.lowerNormal;

		if (timeManager.startTime < 1) {
			simulationSpeed.value = (int)(timeManager.startTime * 10 - 1);
			UpdateSimulationSpeed();
		} else {
			simulationSpeed.value = (int)(timeManager.startTime + 8);
			UpdateSimulationSpeed();
		}
	}

    // Sets the simulation speed in the LidarSensor script to the value in the GUI aswell as setting the handle text of the slider
    public void UpdateSimulationSpeed(){
        timeManager.SetTimeScale(simSpeedSliderValues[(int)simulationSpeed.value]);
        UpdateSliderHandleText(simulationSpeed, simSpeedSliderValues[(int)simulationSpeed.value]);

    }

    // Update slider handle text
    public void UpdateSliderHandleText(Slider slider, float newVal){
        slider.transform.FindChild("Handle Slide Area").FindChild("Handle").FindChild("HandleText").GetComponent<Text>().text = newVal.ToString();
    }

    // Updates tha values of the lidar mimic to the values of the GUI
    public void UpdateLaserMimicValues(){
		OnLidarMenuValChanged ((int)numberOfLasers.value, upperFOV.value, lowerFOV.value, offset.value, upperNormal.value, lowerNormal.value);
	}

	// Creates an event with the final values set in the GUI for the sensor and pointcloud to listen to
	public void SendSettingsToLidar()
	{
        if (PassGuiValsOnStart != null)
        {
            PassGuiValsOnStart((int)numberOfLasers.value, rotationSpeedHz.value, rotationAnglePerStep.value, rayDistance.value,
                upperFOV.value, lowerFOV.value, offset.value, upperNormal.value, lowerNormal.value);
        }

        /*
		sensor.numberOfLasers = (int)numberOfLasers.value;
		sensor.rotationSpeedHz = rotationSpeedHz.value;
		sensor.rotationAnglePerStep = rotationAnglePerStep.value;
		sensor.rayDistance = rayDistance.value;
		sensor.upperFOV = upperFOV.value;
		sensor.lowerFOV = lowerFOV.value;
		sensor.offset = offset.value;
		sensor.upperNormal = upperNormal.value;
		sensor.lowerNormal = lowerNormal.value;*/
	}


}
