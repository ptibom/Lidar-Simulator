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
    public Button simulationButton, visualizationButton, exitButton;
    

	// Use this for initialization
	void Start () {
        simulationButton = GameObject.Find("Simulation").GetComponent<Button>();
        visualizationButton = GameObject.Find("Visualization").GetComponent<Button>();
        exitButton = GameObject.Find("Exit").GetComponent<Button>();

        simulationButton.onClick.AddListener(StartSimulation);
        visualizationButton.onClick.AddListener(StartVisualization);
        exitButton.onClick.AddListener(Exit);
    }

    /// <summary>
    /// Starts the simulation
    /// </summary>
    public void StartSimulation()
    {
        SceneManager.LoadSceneAsync("FinalScene",LoadSceneMode.Single);
    }

    /// <summary>
    /// Starts the visualization
    /// </summary>
    public void StartVisualization()
    {

    }

    /// <summary>
    /// Exits the program.
    /// </summary>
    public void Exit()
    {

    }
}
