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

    void Start()
    {
        gameObject.GetComponent<Toggle>().onValueChanged.AddListener(OnToggle);
    }

    /// <summary>
    /// Invokes an event to tell every listener that the play button has been toggled on
    /// </summary>
    /// <param name="isOn"></param>
    void OnToggle(bool isOn)
    {
        try
        {
            OnPlayToggled(isOn);
        }
        catch (NullReferenceException e)
        {
            Debug.Log("Event has no delegates: " + e);
        }
    }
}