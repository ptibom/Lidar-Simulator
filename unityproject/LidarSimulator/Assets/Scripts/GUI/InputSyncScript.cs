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
		inputField.text = SetValueLength (gameObject.GetComponent<Slider> ().value.ToString ());
	}

	private string SetValueLength(string text){
		if(text.Length > 4){
			text = text.Substring (0, 4);
		}
		return text;
	}


}
