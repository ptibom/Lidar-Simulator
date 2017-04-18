using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @author: Jonathan Jansson
/// </summary>
public class LidarMenuScript : MonoBehaviour {

	public static event UpdateMimicValues OnLidarMenuValChanged;
    public static event PassLidarMenuValues PassGuiValsOnStart;
    public delegate void UpdateMimicValues(int noLasers, float upFOV, float lowFOV, float offset, float upNorm, float lowNorm);
    public delegate void PassLidarMenuValues(int numberOfLasers, float rotationSpeed, float rotationAnglePerStep, float rayDistance, float upperFOV
        , float lowerFOV, float offset, float upperNormal, float lowerNormal);

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

    public TimeManager timeManager;
    public LidarLineMimic lidarLineMimic;

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
		numberOfLasers.value = sensor.numberOfLasers;
		rotationSpeedHz.value = sensor.rotationSpeedHz;
        rotationAnglePerStep.value = sensor.rotationAnglePerStep;
        rayDistance.value = sensor.rayDistance;
        upperFOV.value = sensor.upperFOV;
        lowerFOV.value = sensor.lowerFOV;
        offset.value = sensor.offset;
        upperNormal.value = sensor.upperNormal;
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
    public void UpdateSimulationSpeed()
    {
        timeManager.SetTimeScale(simSpeedSliderValues[(int)simulationSpeed.value]);
        UpdateSliderHandleText(simulationSpeed, simSpeedSliderValues[(int)simulationSpeed.value]);

    }

    // Update slider handle text
    public void UpdateSliderHandleText(Slider slider, float newVal)
    {
        slider.transform.FindChild("Handle Slide Area").FindChild("Handle").FindChild("HandleText").GetComponent<Text>().text = newVal.ToString();
    }

    // Updates tha values of the lidar mimic to the values of the GUI
    public void UpdateLaserMimicValues()
    {
		OnLidarMenuValChanged ((int)numberOfLasers.value, upperFOV.value, lowerFOV.value, offset.value, upperNormal.value, lowerNormal.value);
	}

	// Creates an event with the final values set in the GUI for the sensor and pointcloud to listen to
	public void SendSettingsToLidar()
	{
        if (PassGuiValsOnStart != null) {
            PassGuiValsOnStart((int)numberOfLasers.value, rotationSpeedHz.value, rotationSpeedHz.value, rayDistance.value,
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
