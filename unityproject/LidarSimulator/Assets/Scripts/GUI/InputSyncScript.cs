using System.Collections;
using System.Collections.Generic;
using UnityEngine;	
using UnityEngine.UI;

public class InputSyncScript : MonoBehaviour {

	public GameObject inputField;	

	public void SyncToSlider(){
		if(inputField.GetComponent<InputField> ().text.Equals("")){
			inputField.GetComponent<InputField> ().text = "0";
		}
		gameObject.GetComponent<Slider> ().value = float.Parse(inputField.GetComponent<InputField> ().text);
	}

	public void SyncToInputField(){
		inputField.GetComponent<InputField> ().text = gameObject.GetComponent<Slider> ().value.ToString();
	}

}
