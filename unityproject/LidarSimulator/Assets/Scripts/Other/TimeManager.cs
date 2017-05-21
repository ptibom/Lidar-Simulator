using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// @Author: Philip Tibom
/// </summary>
public class TimeManager : MonoBehaviour {

	public float startTime = 1;
    private float oldTime = 1;

	// Use this for initialization
	void Start () {
        Time.fixedDeltaTime = 0.0002f; // Necessary for simulation to be detailed. Default is 0.02f.
        SetTimeScale(startTime);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void PauseTime()
    {
        oldTime = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void UnPauseTime()
    {
        Time.timeScale = oldTime;
    }
}
