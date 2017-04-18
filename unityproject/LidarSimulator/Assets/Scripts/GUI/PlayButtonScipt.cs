using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A script which controls all switching of views and UI elements depending on play/stop state
/// @author: Jonathan Jansson
/// </summary>
public class PlayButtonScipt : MonoBehaviour {

    public EditorControllerScript editorController;
    public Toggle lidarSensorButton;

	public void OnToggle(){
		editorController.SwitchMainCameraView (gameObject.GetComponent<Toggle> ().isOn);
		editorController.SetLidarCameraActive (lidarSensorButton.isOn && !gameObject.GetComponent<Toggle> ().isOn);
		editorController.SetVisToggleActive (gameObject.GetComponent<Toggle> ().isOn);
	}

}