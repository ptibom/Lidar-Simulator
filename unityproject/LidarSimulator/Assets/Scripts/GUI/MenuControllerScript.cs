using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class MenuControllerScript : MonoBehaviour {

	[SerializeField]
	private LidarSensor sensor;
	[SerializeField]
	private GameObject mainCamera;
	[SerializeField]
	private GameObject lidarCamera;
	[SerializeField]
	private GameObject editorMenu;
	[SerializeField]
	private GameObject pointCloud;
	[SerializeField]
	private GameObject visToggle;

	public void SetLidarCameraActive(bool popup){
        if (popup)
        {
            lidarCamera.SetActive(true);
        } else
        {
            lidarCamera.SetActive(false);
        }
	}

	public void SetMainCamera(bool follow){
		if(follow){
			editorMenu.SetActive (false);
			mainCamera.GetComponent<CameraController> ().SetFollow ();
		}
		else{
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
