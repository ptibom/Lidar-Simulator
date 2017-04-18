using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @author: Jonathan Jansson
/// </summary>

public class EditorControllerScript : MonoBehaviour {

    public LidarSensor sensor;
    public GameObject mainCamera;
    public GameObject lidarCamera;
    public GameObject editorMenu;
    public GameObject pointCloud;
    public GameObject visToggle;
    public Toggle pauseToggle;



	public void SetLidarCameraActive(bool popup){
        if (popup)
        {
            lidarCamera.SetActive(true);
        } else
        {
            lidarCamera.SetActive(false);
        }
	}

	public void SwitchMainCameraView(bool follow){
		if(follow){
			editorMenu.SetActive (false);
			mainCamera.GetComponent<CameraController> ().SetFollow ();
		}
		else{
            if (pauseToggle.isOn)
            {
                pauseToggle.isOn = false;
            }
			editorMenu.SetActive (true);
			mainCamera.GetComponent<CameraController> ().SetRoam ();
		}
	}

	public void SetVisToggleActive(bool setOn){
		if(setOn){
			visToggle.SetActive (true);	
			if(visToggle.GetComponent<Toggle>().isOn && !pointCloud.activeInHierarchy){
				TogglePointCloudActive ();
			}
		} else {
			visToggle.SetActive (false);
			if(pointCloud.activeInHierarchy){
				TogglePointCloudActive ();
			}
		}
	}

    public void TogglePointCloudActive()
    {	
		if (pointCloud.activeInHierarchy)
        {
			pointCloud.SetActive (false);
           	mainCamera.GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
        } else {
			pointCloud.SetActive (true);
            mainCamera.GetComponent<Camera>().rect = new Rect(0, 0 , 0.5f, 1);
        }
        	mainCamera.GetComponent<Camera>().enabled = true;
    }
}
