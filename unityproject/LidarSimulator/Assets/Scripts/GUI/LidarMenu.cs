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

    public delegate void UpdateMimicValuesDelegate(int noLasers, float upFOV, float lowFOV, float offset, float upNorm, float lowNorm);
    public delegate void PassLidarValuesToPointCloudDelegate(int numberOfLasers, float rotationSpeed, float rotationAnglePerStep);
    public delegate void StartSimulationDelegate(int numberOfLasers, float rotationSpeed, float rotationAnglePerStep, float rayDistance, float upperFOV
        , float lowerFOV, float offset, float upperNormal, float lowerNormal);
    public delegate void StopSimulationDelegate();

    public static event UpdateMimicValuesDelegate OnPassValuesToLaserMimic;
    public static event PassLidarValuesToPointCloudDelegate OnPassLidarValuesToPointCloud;
    public static event StartSimulationDelegate OnPassValuesToLidarSensor;
    public static event StopSimulationDelegate OnStopSimulation;

    public Slider numberOfLasers;
    public Slider rotationSpeedHz;
    public Slider rotationAnglePerStep;
    public Slider rayDistance;
    public Slider upperFOV;
    public Slider lowerFOV;
    public Slider offset;
    public Slider upperNormal;
    public Slider lowerNormal;

    public float numberOfLasersVal = 64f;
    public float rotationSpeedHzVal = 1f;
    public float rotationAnglePerStepVal = 0.9f;
    public float rayDistanceVal = 200f;
    public float upperFOVVal = 10.5f;
    public float lowerFOVVal = 16f;
    public float offsetVal = 7.24f;
    public float upperNormalVal = -3.3f;
    public float lowerNormalVal = 16.87f;

    public PreviewLidarRays lidarLinePreview;

    private bool laserMimicInitialized = false;
    private bool guiValsInitialized = false;

    void Awake()
    {
        EditorController.OnPointCloudToggle += PassLidarValuesToPointCloud;
        PlayButton.OnPlayToggled += PassValuesToLidarSensor;
        PreviewLidarRays.tellLidarMenuInitialized += LaserMimicIsInitialized;
    }

    /// <summary>
    /// Calls all initialization methods which syncs the lidar menu sliders with the lidar sensors initial values, and creates and updates the lidar mimic system.
    /// </summary>
	void Start () 
	{
        InitializeGUIValues();
        if (laserMimicInitialized)
        {
            UpdateLaserMimicValues();
        }
    }

    void OnDestroy()
    {
        EditorController.OnPointCloudToggle -= PassLidarValuesToPointCloud;
        PlayButton.OnPlayToggled -= PassValuesToLidarSensor;
        PreviewLidarRays.tellLidarMenuInitialized -= LaserMimicIsInitialized;
    }

    /// <summary>
    /// Sets all initial values in the lidar menu.
    /// </summary>
	void InitializeGUIValues()
	{
		numberOfLasers.value = numberOfLasersVal;
		rotationSpeedHz.value = rotationSpeedHzVal;
        rotationAnglePerStep.value = rotationAnglePerStepVal;
        rayDistance.value = rayDistanceVal;
        upperFOV.value = upperFOVVal;
        lowerFOV.value = lowerFOVVal;
        offset.value = offsetVal;
        upperNormal.value = upperNormalVal;
        lowerNormal.value = lowerNormalVal;

        guiValsInitialized = true;
	}

    void LaserMimicIsInitialized(bool isInitialized)
    {
        laserMimicInitialized = true;
        if (guiValsInitialized)
        {
            UpdateLaserMimicValues();
        }
    }

    /// <summary>
    /// Invokes an event to updates the values of the lidar mimic to the values of the GUI
    /// </summary>
    void UpdateLaserMimicValues()
    {
        if (laserMimicInitialized)
        {
            try
            {
                OnPassValuesToLaserMimic((int)numberOfLasers.value, upperFOV.value, lowerFOV.value, offset.value, upperNormal.value, lowerNormal.value);
            }
            catch (NullReferenceException e)
            {
                Debug.Log("Event has no delegates: " + e);
            }
        }
    }


    /// <summary>
    /// Invokes an event with the current values set in the GUI for the lidar sensor and pointcloud to listen to
    /// </summary>
    void PassValuesToLidarSensor(bool play)
	{
        if (play)
        {
            try
            {
                OnPassValuesToLidarSensor((int)numberOfLasers.value, rotationSpeedHz.value, rotationAnglePerStep.value, rayDistance.value,
                    upperFOV.value, lowerFOV.value, offset.value, upperNormal.value, lowerNormal.value);
            }
            catch (NullReferenceException e)
            {
                Debug.Log("Event has no delegates: " + e);
            }
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
