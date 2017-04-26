using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Calls correct timeControl methods from the pause toggle
/// 
/// @author: Jonathan Jansson
/// </summary>
public class PauseToggle : MonoBehaviour {

    public TimeManager timeController;

	public void Toggle()
    {
		if(gameObject.GetComponent<Toggle>().isOn)
        {
			timeController.PauseTime();
		}
        else
        {
			timeController.UnPauseTime();
		}
	}
}
