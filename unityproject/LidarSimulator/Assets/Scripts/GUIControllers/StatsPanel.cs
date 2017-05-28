using System;
using System.Collections.Generic;
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
    public Text tickText;

	private float updateDelay = 0.25f;
	private float updateTime = 0f;
    private bool simulationModeOn = false;

	private float timeCounter = 0f;
	private float frameCounter = 0f;

    private int tickCounter = 0;

    private float startTime = 0f;

    private int pointsHit = 0;
    private string pointsHitText;

    private void Awake()
    {
        PlayButton.OnPlayToggled += Reset;
        LidarSensor.OnScanned += UpdatePointsHitCounter;
    }

    void OnDestroy()
    {
        PlayButton.OnPlayToggled -= Reset;
        LidarSensor.OnScanned -= UpdatePointsHitCounter;
    }

    void UpdatePointsHitCounter(float time, LinkedList<SphericalCoordinate> hits)
    {
        pointsHit += hits.Count;
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

            if (Time.time > updateTime)
            {
                UpdateTexts();
                timeCounter = Time.time;
            }
        }
    }

    void FixedUpdate()
    {
        tickCounter += 1;
    }

    /// <summary>
    /// Calculates and sets all parameters of the GUI stats panel
    /// </summary>
	void UpdateTexts()
    {
        float deltaTime = Time.time - timeCounter;
		timeText.text = "Time: " + ((int)(Time.time - startTime)).ToString() + " s";	
		hitPText.text = "Points hit: " + UpdatePointsHitPrefixText();
		fpsText.text = "Fps: " + (int)(frameCounter / deltaTime);
        tickText.text = "Ticks: " + (int)(tickCounter/deltaTime) + "/s";
		frameCounter = 0f;
        tickCounter = 0;
		updateTime = Time.time + updateDelay;
	}

    String UpdatePointsHitPrefixText()
    {
        string pointsHitTmp;
        if (pointsHit < 1000)
        {
            return pointsHit.ToString();
        }
        else if(pointsHit < 1000000)
        {
            pointsHitTmp = ((float)pointsHit / 1000).ToString();
            if (pointsHitTmp.Length <= 5)
            {
                return pointsHitTmp + " K";
            }
            else
            {
                return pointsHitTmp.Substring(0, 5) + " K";
            }
        }
        else
        {
            pointsHitTmp = ((float)pointsHit / 1000000).ToString();
            if (pointsHitTmp.Length <= 4)
            {
                return pointsHitTmp + " M";
            }
            else
            {
                return pointsHitTmp.Substring(0, 4) + " M";
            }
        }
    }
}
