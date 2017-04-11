using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour {

	public TimeManager timeController;

	public void PauseToggle(){
		if(gameObject.GetComponent<Toggle>().isOn){
			timeController.PauseTime();
		} else {
			timeController.UnPauseTime();
		}

	}
}
