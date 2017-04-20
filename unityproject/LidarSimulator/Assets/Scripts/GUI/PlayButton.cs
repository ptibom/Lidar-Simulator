using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlls all switching of views and UI elements depending on play/stop state. 
/// Also controlls when the lidarMenu script sends new values
/// 
/// @author: Jonathan Jansson
/// </summary>
public class PlayButton : MonoBehaviour {

    public EditorController editorController;
    public LidarMenu lidarMenu;
    public Toggle lidarSensorButton;

    // SUPER QUICK WORKAROUND FIX BELOW!!!!!!!!!!!!!!!!!

	public void OnToggle()
    {
        bool toggleIsOn = gameObject.GetComponent<Toggle>().isOn;
        editorController.SetMode(toggleIsOn);
    }

}