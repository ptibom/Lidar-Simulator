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

	public static event UpdateMimicValuesDelegate OnLidarMenuValChanged;
    public static event PassLidarValuesToPointCloudDelegate OnPassLidarValuesToPointCloud;
    public static event StartSimulationDelegate OnStartSimulation;
    public static event StopSimulationDelegate OnStopSimulation;
    public delegate void UpdateMimicValuesDelegate(int noLasers, float upFOV, float lowFOV, float offset, float upNorm, float lowNorm);
    public delegate void PassLidarValuesToPointCloudDelegate(int numberOfLasers, float rotationSpeed, float rotationAnglePerStep);
    public delegate void StartSimulationDelegate(int numberOfLasers, float rotationSpeed, float rotationAnglePerStep, float rayDistance, float upperFOV
        , float lowerFOV, float offset, float upperNormal, float lowerNormal);
    public delegate void StopSimulationDelegate();

	public Slider numberOfLasers;
    public Slider rotationSpeedHz;
    public Slider rotationAnglePerStep;
    public Slider rayDistance;
    public Slider upperFOV;
    public Slider lowerFOV;
    public Slider offset;
    public Slider upperNormal;
    public Slider lowerNormal;

    public PreviewLidarRays lidarLinePreview;

    private LidarSensor sensor;


    /// <summary>
    /// Calls all initialization methods which syncs the lidar menu sliders with the lidar sensors initial values, and creates and  updates the lidar mimic system.
    /// </summary>
	void Start () 
	{
        PlayButton.OnPlayToggled += StartSimulation;
        EditorController.OnPointCloudToggle += PassLidarValuesToPointCloud;

        sensor = GameObject.FindGameObjectWithTag("Lidar").GetComponent<LidarSensor>();
		lidarLinePreview.InitializeLaserMimicList ();
        InitializeGUIValues();
        UpdateLaserMimicValues();
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
    void UpdateLaserMimicValues()
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
    /// Sends an event to let other scripts know to stop simulation mode
    /// </summary>
    void StopSimulation()
    {
        try
        {
            OnStopSimulation();
        }
        catch (NullReferenceException e)
        {
            Debug.Log("Event has no delegates: " + e);
        }
    }

    /// <summary>
    /// Invokes an event with the current values set in the GUI for the lidar sensor and pointcloud to listen to
    /// </summary>
    void StartSimulation(bool play)
	{
        if (play)
        {
            try
            {
                OnStartSimulation((int)numberOfLasers.value, rotationSpeedHz.value, rotationAnglePerStep.value, rayDistance.value,
                    upperFOV.value, lowerFOV.value, offset.value, upperNormal.value, lowerNormal.value);
            }
            catch (NullReferenceException e)
            {
                Debug.Log("Event has no delegates: " + e);
            }
        }
        else
        {
            StopSimulation();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void PassLidarValuesToPointCloud()
    {
        try
        {
            OnPassLidarValuesToPointCloud((int)numberOfLasers.value, rotationSpeedHz.value, rotationAnglePerStep.value);
        }
        catch (NullReferenceException e)
        {
            Debug.Log("Event has no delegates: " + e);
        }
    }
}
