using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class MenuControllerScript : MonoBehaviour {

	LidarSensor sensor;
	GameObject mainCamera;
	GameObject lidarCamera;
    GameObject visCamera;
	// Use this for initialization
	void Start () {
		sensor = GameObject.FindGameObjectWithTag ("Lidar").GetComponent<LidarSensor> ();
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		lidarCamera = GameObject.Find ("LidarCamera");
        visCamera = GameObject.FindGameObjectWithTag("VisCamera");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayButton(){
		//sensor.playSimulation = !sensor.playSimulation;
	}
		
	public void SwitchCamera(bool popup){
        if (popup)
        {
            lidarCamera.SetActive(true);
        } else
        {
            lidarCamera.SetActive(false);
        }
	}

    public void SwitchVisualisationCamera()
    {
        if (visCamera.activeInHierarchy)
        {
            visCamera.SetActive(false);
            mainCamera.GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
        } else {
            visCamera.SetActive(true);
            mainCamera.GetComponent<Camera>().rect = new Rect(0, 0 , 0.5f, 1);
        }
        mainCamera.GetComponent<Camera>().enabled = true;
    }
}
