using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the simulationspeed slider and its conection to the timeManager
/// 
/// @author: Jonathan Jansson
/// </summary>
public class SimSpeed : MonoBehaviour {

    public TimeManager timeManager;
    public Text handleText;

    private Slider slider;
    private float[] simSpeedSliderValues = { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    /// <summary>
    /// Syncs the slider to the initial values of the time manager by converting the value from the 
    /// time manager to its corresponding position in the simSpeedSliderValues list.
    /// </summary>
    void Start ()
    {
        slider = gameObject.GetComponent<Slider>();

        if (timeManager.startTime < 1)
        {
            slider.value = (int)(timeManager.startTime * 10 - 1);
            UpdateSimulationSpeed();
        }
        else
        {
            slider.value = (int)(timeManager.startTime + 8);
            UpdateSimulationSpeed();
        }
    }

    /// <summary>
    /// Sets the timescale in the time manager and the value displayed on the slider handle to the position 
    /// in the simSpeedSliderValues list of the value of the slider
    /// </summary>
    public void UpdateSimulationSpeed()
    {
        float newVal = simSpeedSliderValues[(int)slider.value];
        timeManager.SetTimeScale(newVal);
        UpdateSliderHandleText(newVal);
    }

    /// <summary>
    /// Updates the text displayed on the sliders handle
    /// </summary>
    /// <param name="newVal"></param>
    public void UpdateSliderHandleText(float newVal)
    {
        handleText.text = newVal.ToString();
    }
}
