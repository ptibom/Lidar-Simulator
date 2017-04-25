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
    public Button simulationButton, visualizationButton, exitButton ;
    public GameObject loadWheel, loadingTextObject, buttonPanel, loadPanel;

    private Text loadProgress;
    private AsyncOperation async = null; 
    


    // Use this for initialization
    void Start () {
        simulationButton = GameObject.Find("Simulation").GetComponent<Button>();
        visualizationButton = GameObject.Find("Visualization").GetComponent<Button>();
        exitButton = GameObject.Find("Exit").GetComponent<Button>();
        loadWheel = GameObject.Find("LoadImage");
        loadingTextObject = GameObject.Find("LoadText");
        buttonPanel = GameObject.Find("ButtonPanel");
        loadPanel = GameObject.Find("LoadingPanel");

        loadProgress = loadingTextObject.GetComponent<Text>();
        simulationButton.onClick.AddListener(StartSimulation);
        visualizationButton.onClick.AddListener(StartVisualization);
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
            loadProgress.text = (Mathf.Ceil(async.progress) *100).ToString() + "%";
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
    /// Exits the program.
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }
}
