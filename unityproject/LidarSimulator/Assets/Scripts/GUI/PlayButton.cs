using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlls all switching of views and UI elements depending on play/stop state
/// 
/// @author: Jonathan Jansson
/// </summary>
public class PlayButton : MonoBehaviour {

    public EditorController editorController;
    public Toggle lidarSensorButton;

	public void OnToggle()
    {
        editorController.SetMode(gameObject.GetComponent<Toggle>().isOn);
	}

}