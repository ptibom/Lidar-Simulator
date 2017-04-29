using System;
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
    private bool simulationModeOn = false;

	private float timeCounter = 0f;
	private float frameCounter = 0f;

    private float startTime = 0f;

    private int pointsHit = 0;

    private void Awake()
    {
        PlayButton.OnPlayToggled += Reset;
    }


    /// <summary>
    /// Resets relevant values and starts the updating of stats when simulation starts
    /// </summary>
    /// <param name="simulationMode"></param>
    private void Reset(bool simulationMode)
    {
        simulationModeOn = simulationMode;
        if (simulationMode)
        {
            startTime = Time.fixedTime;
            pointsHit = 0;
            frameCounter = 0f;
            timeCounter = Time.fixedTime;
            updateTime = Time.fixedTime + updateDelay;
        }
    }

    /// <summary>
    /// Counts frames for the fps counter every update and handles the delay between GUI updates of values
    /// </summary>
    void Update ()
    {
        if (simulationModeOn)
        {
            frameCounter += 1;

            if (Time.fixedTime > updateTime)
            {
                UpdateTexts();
                timeCounter = Time.fixedTime;
            }
        }
    }

    /// <summary>
    /// Calculates and sets all parameters of the GUI stats panel
    /// </summary>
	void UpdateTexts()
    {
        float deltaTime = Time.fixedTime - timeCounter;
		timeText.text = "Time: " + ((int)(Time.fixedTime - startTime)).ToString() + " s";	
		hitPText.text = "Points hit: " + pointsHit;
		fpsText.text = "Fps: " + (int)(frameCounter / deltaTime);
		frameCounter = 0f;
		updateTime = Time.fixedTime + updateDelay;
	}
}
