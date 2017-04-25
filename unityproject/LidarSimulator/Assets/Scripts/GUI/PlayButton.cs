using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlls all switching of views and UI elements depending on Play/Stop state. 
/// Also invokes events to allert other scripts to act when toggling Play/Stop
/// 
/// @author: Jonathan Jansson
/// </summary>
public class PlayButton : MonoBehaviour {

    public static event PlayToggledDelegate OnPlayToggled;
    public delegate void PlayToggledDelegate(bool toggleIsOn);

    public EditorController editorController;
    public LidarMenu lidarMenu;
    public Toggle lidarSensorButton;

    public void OnToggle()
    {
        bool toggleIsOn = gameObject.GetComponent<Toggle>().isOn;
        try
        {
            OnPlayToggled(toggleIsOn);
        }
        catch (NullReferenceException e)
        {
            Debug.Log("Event has no delegates: " + e);
        }


    }
}