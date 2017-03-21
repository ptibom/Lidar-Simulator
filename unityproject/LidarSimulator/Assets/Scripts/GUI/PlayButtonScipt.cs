using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonScipt : MonoBehaviour {

	public GameObject editorMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){
		if(gameObject.GetComponent<Toggle>().isOn){
			editorMenu.SetActive (false);
		}
		else{
			editorMenu.SetActive (true);
		}
	}

}
