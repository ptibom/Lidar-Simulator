using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlls the activity of all cameras and GUI settings in consideration to the cameras states
/// 
/// @author: Jonathan Jansson
/// </summary>

public class EditorController : MonoBehaviour{

    public LidarSensor sensor;
    public GameObject mainCamera;
    public GameObject lidarCamera;
    public GameObject editorMenu;
    public GameObject pointCloud;
    public GameObject visToggle;
    public Toggle pauseToggle;
    public Toggle lidarSensorToggle;

    /// <summary>
    /// Sets activity of all necessary objects depending on if smulator or editor mode should be "on"
    /// </summary>
    /// <param name="simulatorMode"></param>
    public void SetMode(bool simulatorMode)
    {
        if (!simulatorMode)
        {
            pauseToggle.isOn = false;
        }
        
        SwitchMainCameraView(simulatorMode);
        pauseToggle.interactable = simulatorMode;
        editorMenu.SetActive(!simulatorMode);
        lidarCamera.SetActive(lidarSensorToggle.isOn && !simulatorMode);
        SetVisToggleActive(simulatorMode);
    }

    /// <summary>
    /// Sets the veiw and control settings of the main camera
    /// </summary>
    /// <param name="follow"></param>
	void SwitchMainCameraView(bool follow)
    {
		if(follow)
        {
			mainCamera.GetComponent<CameraController> ().SetFollow ();
		}
		else
        {
			mainCamera.GetComponent<CameraController> ().SetRoam ();
		}
	}

    /// <summary>
    /// Toggles the state of the point cloud toggle object and sets the activity of the point cloud object accordingly
    /// </summary>
    /// <param name="state"></param>
	void SetVisToggleActive(bool state)
    {
        visToggle.SetActive(state);
        
        bool visToggleOn = visToggle.GetComponent<Toggle>().isOn;
        bool pointCloudActive = pointCloud.activeInHierarchy;

        if ((state && visToggleOn && !pointCloudActive) || (!state && pointCloudActive))
        {
            TogglePointCloudActive();
        }
	}

    /// <summary>
    /// Toggles the split screen visualisation through handling the activity of the point cloud and sets the main camera size to split or non split screen size
    /// </summary>
    void TogglePointCloudActive()
    {
        pointCloud.SetActive(!pointCloud.activeInHierarchy);    

		if (!pointCloud.activeInHierarchy)
        {
           	mainCamera.GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
        }
        else
        {
            mainCamera.GetComponent<Camera>().rect = new Rect(0, 0 , 0.5f, 1);
        }

        mainCamera.GetComponent<Camera>().enabled = true;
    }
}
