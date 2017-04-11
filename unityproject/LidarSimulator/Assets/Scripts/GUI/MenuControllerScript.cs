using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class MenuControllerScript : MonoBehaviour {

	public LidarSensor sensor;
	public GameObject mainCamera;
	public GameObject lidarCamera;
	public GameObject editorMenu;
	public GameObject visToggle;
	public GameObject pointCloud;

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
				SwitchPointCloudActive ();
			}
		} else {
			visToggle.SetActive (false);
			if(pointCloud.activeInHierarchy){
				SwitchPointCloudActive ();
			}
		}
	}

    public void SwitchPointCloudActive()
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
