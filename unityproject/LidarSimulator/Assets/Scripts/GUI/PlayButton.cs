using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlls all switching of views and UI elements depending on play/stop state. 
/// Also controlls when the lidarMenu script sends new values
/// 
/// @author: Jonathan Jansson
/// </summary>
public class PlayButton : MonoBehaviour {

    public static event PlayDelegate OnPlay;
    public static event StopDelegate OnStop;
    public delegate void PlayDelegate();
    public delegate void StopDelegate();
    public EditorController editorController;
    public LidarMenu lidarMenu;
    public Toggle lidarSensorButton;

    // SUPER QUICK WORKAROUND FIX BELOW!!!!!!!!!!!!!!!!!

    public void OnToggle()
    {
        bool toggleIsOn = gameObject.GetComponent<Toggle>().isOn;
        editorController.SetMode(toggleIsOn);
        if (toggleIsOn)
        {
            try
            {
                OnPlay();
            }
            catch (NullReferenceException e)
            {
                Debug.Log("Event has no delegates: " + e);
            }
        }
        else
        {
            try
            {
                OnStop();
            }
            catch (NullReferenceException e)
            {
                Debug.Log("Event has no delegates: " + e);
            }
        }
    }
}