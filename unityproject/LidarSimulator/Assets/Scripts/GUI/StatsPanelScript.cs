using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatsPanelScript : MonoBehaviour {

	private Text timeText;
	private Text fpsText;
	private Text hitPText;

	private float updateDelay = 0.25f;
	private float lastUpdateTime = 0f;

	private float timeCounter = 0f;
	private float frameCounter = 0f;

	// Use this for initialization
	void Start () {
		timeText = GameObject.Find ("TimeText").GetComponent<Text> ();
		fpsText = GameObject.Find ("FpsText").GetComponent<Text> ();
		hitPText = GameObject.Find ("HitPText").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		timeCounter += Time.deltaTime;
		frameCounter += 1;

		UpdateText ();
	}

	void UpdateText(){
		if(Time.fixedTime > lastUpdateTime){
			timeText.text = "Time: " + Time.fixedTime.ToString ("F") + " s";	
			hitPText.text = "Points hit: " + 0;
			fpsText.text = "Fps: " + (int)(frameCounter / timeCounter);

			timeCounter = 0f;
			frameCounter = 0f;

			lastUpdateTime = Time.fixedTime + updateDelay;
		}
	}
}
