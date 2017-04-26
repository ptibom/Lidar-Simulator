using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveBtn : MonoBehaviour {

    Button saveBtn;
    LidarStorage ls;

    // Use this for initialization
    void Start () {
        saveBtn = GameObject.Find("SaveButton").GetComponent<Button>();
        saveBtn.onClick.AddListener(SaveData);
        ls = GameObject.FindGameObjectWithTag("Lidar").GetComponent<LidarStorage>();
	}
	
	// Update is called once per frame
	void SaveData () {
        string dataPath = Application.persistentDataPath + "/test.lidardata";
        SaveManager.SaveToCsv(ls.GetData(),dataPath);
	}
}
