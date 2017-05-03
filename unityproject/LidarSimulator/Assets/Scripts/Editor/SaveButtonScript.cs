using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveButtonScript : MonoBehaviour {
    public GameObject saveBtnGameObject;
    public GameObject fileBrowser;
    private Button saveButton;



	// Use this for initialization
	void Start () {
        saveBtnGameObject = GameObject.Find("SaveButton");
        fileBrowser = GameObject.Find("FileBrowser");
        saveButton = saveBtnGameObject.GetComponent<Button>();
        saveButton.onClick.AddListener(InitializeSave);
	}
	
	public void InitializeSave()
    {
        fileBrowser.GetComponent<TestFileBrowser>().ToggleFileBrowser();
    }
}
