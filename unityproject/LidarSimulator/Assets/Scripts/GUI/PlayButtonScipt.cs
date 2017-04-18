using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonScipt : MonoBehaviour {

	[SerializeField]
	private MenuControllerScript menuController;
	[SerializeField]
	private Toggle lidarSensorButton;

	public void OnToggle(){
		menuController.SetMainCamera (gameObject.GetComponent<Toggle> ().isOn);
		menuController.SetLidarCameraActive (lidarSensorButton.isOn && !gameObject.GetComponent<Toggle> ().isOn);
		menuController.SetVisToggleActive (gameObject.GetComponent<Toggle> ().isOn);
	}

}