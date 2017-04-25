using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlls and updates all parameters of the stats window
/// 
/// @author: Jonathan Jansson
/// </summary>
public class StatsPanel : MonoBehaviour {

    public Text fpsText;
    public Text timeText;
	public Text hitPText;

	private float updateDelay = 0.25f;
	private float updateTime = 0f;
    private float lastUpdate = 0f;

	private float timeCounter = 0f;
	private float frameCounter = 0f;


	/// <summary>
    /// Counts frames for the fps counter every update and handles the delay between GUI updates of values
    /// </summary>
	void Update ()
    {
		frameCounter += 1;

        if (Time.fixedTime > updateTime)
        {
            UpdateTexts();
            timeCounter = Time.time;
        }
    }

    /// <summary>
    /// Calculates and sets all parameters of the GUI stats panel
    /// </summary>
	void UpdateTexts()
    {
        float deltaTime = Time.time - timeCounter;
		timeText.text = "Time: " + ((int)Time.fixedTime).ToString() + " s";	
		hitPText.text = "Points hit: " + 0;
		fpsText.text = "Fps: " + (int)(frameCounter / deltaTime);
		frameCounter = 0f;
		updateTime = Time.fixedTime + updateDelay;
	}
}
