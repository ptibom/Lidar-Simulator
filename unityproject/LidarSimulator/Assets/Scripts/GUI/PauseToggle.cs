using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Calls correct timeControl methods from the pause toggle
/// 
/// @author: Jonathan Jansson
/// </summary>
public class PauseToggle : MonoBehaviour {

    public TimeManager timeController;

    void Start()
    {
        gameObject.GetComponent<Toggle>().onValueChanged.AddListener(Toggle);
    }

    void Toggle(bool isOn)
    {
		if(isOn)
        {
			timeController.PauseTime();
		}
        else
        {
			timeController.UnPauseTime();
		}

	}
}
