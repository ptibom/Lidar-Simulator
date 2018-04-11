/*
* MIT License
* 
* Copyright (c) 2017 Philip Tibom, Jonathan Jansson, Rickard Laurenius, 
* Tobias Alldén, Martin Chemander, Sherry Davar
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// Script used for the main menu of the application.
/// author: Tobias Alldén
/// </summary>
public class MainMenuScript : MonoBehaviour {
    public Button simulationButton, visualizationButton, creditsButton, exitButton ;
    public GameObject loadWheel, loadingTextObject, buttonPanel, loadPanel;

    private Text loadProgress;
    private AsyncOperation async = null; 
    


    // Use this for initialization
    void Start () {
        simulationButton = GameObject.Find("Simulation").GetComponent<Button>();
        visualizationButton = GameObject.Find("Visualization").GetComponent<Button>();
        exitButton = GameObject.Find("Exit").GetComponent<Button>();
        creditsButton = GameObject.Find("Credits").GetComponent<Button>();
        loadWheel = GameObject.Find("LoadImage");
        loadingTextObject = GameObject.Find("LoadText");
        buttonPanel = GameObject.Find("ButtonPanel");
        loadPanel = GameObject.Find("LoadingPanel");

        loadProgress = loadingTextObject.GetComponent<Text>();
        simulationButton.onClick.AddListener(StartSimulation);
        visualizationButton.onClick.AddListener(StartVisualization);
        creditsButton.onClick.AddListener(StartCredits);
        exitButton.onClick.AddListener(Exit);

        loadPanel.SetActive(false);

    }

    private IEnumerator LoadALevel(string levelName)
    {
        async = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
        yield return async;
        
    }


    private void Update()
    {
        if(async != null)
        {
            loadWheel.transform.Rotate(Vector3.back * Time.deltaTime * 300);
            loadProgress.text = (async.progress *100).ToString() + "%";
        }
    }

    /// <summary>
    /// Starts the simulation
    /// </summary>
    public void StartSimulation()
    {
        loadPanel.SetActive(true);
        buttonPanel.SetActive(false);
        StartCoroutine(LoadALevel("FinalScene"));

    }

    /// <summary>
    /// Starts the visualization
    /// </summary>
    public void StartVisualization()
    {
        loadPanel.SetActive(true);
        buttonPanel.SetActive(false);
        StartCoroutine(LoadALevel("ExternalVisualization"));
        
    }

    /// <summary>
    /// Starts the Credits
    /// </summary>
    public void StartCredits()
    {
        loadPanel.SetActive(true);
        buttonPanel.SetActive(false);
        StartCoroutine(LoadALevel("Credits"));

    }

    /// <summary>
    /// Exits the program.
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }
}
