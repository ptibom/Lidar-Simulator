using System.Collections;
using System.Collections.Generic;
using UnityEngine;	
using UnityEngine.UI;

public class InputSyncScript : MonoBehaviour {

	public InputField inputField;	

	public void AvoidEmptyInput(){
		if(inputField.text.Equals("")){
			inputField.text = "0";
		} else if(gameObject.GetComponent<Slider> ().maxValue < float.Parse(inputField.text)){
			inputField.text = gameObject.GetComponent<Slider> ().maxValue.ToString ();
		}
	}

	public void SyncToSlider(){
		gameObject.GetComponent<Slider> ().value = float.Parse(inputField.text);
	}

	public void SyncToInputField(){



		//FIX! This does not set the length of the string!!!



		inputField.text = gameObject.GetComponent<Slider>().value.ToString();
	}

}
