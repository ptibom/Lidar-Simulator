using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles almost all interactions between the lidar menu and all other components of the simulator.
/// Main functionality: Sends the settings of the lidar menu GUI to the LidarLaserMimic, LidarSensor and the PointCloud.
/// 
/// @author: Jonathan Jansson
/// </summary>
public class LidarMenu : MonoBehaviour {

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

    public LidarLineMimic lidarLineMimic;

    private LidarSensor sensor;


    /// <summary>
    /// Calls all initialization methods which syncs the lidar menu sliders with the lidar sensors initial values, and creates and  updates the lidar mimic system.
    /// </summary>
	void Start () 
	{
        sensor = GameObject.FindGameObjectWithTag("Lidar").GetComponent<LidarSensor>();
		lidarLineMimic.InitializeLaserMimicList ();
		InitializeGUIValues ();
		UpdateLaserMimicValues ();
	}
		
    /// <summary>
    /// Initializes all the values of the lidar menu with the initial values of the lidar sensor.
    /// </summary>
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
	}

    /// <summary>
    /// Invokes an event to updates the values of the lidar mimic to the values of the GUI
    /// </summary>
    public void UpdateLaserMimicValues()
    {
        try
        {
            OnLidarMenuValChanged((int)numberOfLasers.value, upperFOV.value, lowerFOV.value, offset.value, upperNormal.value, lowerNormal.value);
        }
        catch (NullReferenceException e)
        {
            Debug.Log("Event has no delegates: " + e);
        }
    }

    /// <summary>
    /// Invokes an event with the current values set in the GUI for the lidar sensor and pointcloud to listen to
    /// </summary>
    public void SendSettingsToLidar()
	{
        try
        {
            PassGuiValsOnStart((int)numberOfLasers.value, rotationSpeedHz.value, rotationAnglePerStep.value, rayDistance.value,
                upperFOV.value, lowerFOV.value, offset.value, upperNormal.value, lowerNormal.value);
        }
        catch (NullReferenceException e)
        {
            Debug.Log("Event has no delegates: " + e);
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
