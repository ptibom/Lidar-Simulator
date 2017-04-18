using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for calling correct timeControl methods from the pause toggle
/// @author: Jonathan Jansson
/// </summary>
public class PauseToggleScript : MonoBehaviour {

    public TimeManager timeController;

	public void PauseToggle(){
		if(gameObject.GetComponent<Toggle>().isOn){
			timeController.PauseTime();
		} else {
			timeController.UnPauseTime();
		}

	}
}
