using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonScipt : MonoBehaviour {

	public MenuControllerScript menuController;
	public Toggle lidarSensorButton;

	void OnGUI(){
		menuController.SetMainCamera (gameObject.GetComponent<Toggle> ().isOn);
		menuController.SetLidarCameraActive (lidarSensorButton.isOn && !gameObject.GetComponent<Toggle> ().isOn);
	}

}
