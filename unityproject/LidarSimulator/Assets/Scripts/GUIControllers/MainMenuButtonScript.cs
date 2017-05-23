using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButtonScript : MonoBehaviour {
    private Button toMainMenu;

	// Use this for initialization
	void Start () {
        toMainMenu = GameObject.Find("MainMenuButton").GetComponent<Button>();
        toMainMenu.onClick.AddListener(LoadMain);


    }

    // Update is called once per frame
    void LoadMain () {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
