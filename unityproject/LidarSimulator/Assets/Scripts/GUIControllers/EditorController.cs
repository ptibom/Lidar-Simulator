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

using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlls the activity of all cameras and GUI settings in consideration to the cameras states
/// 
/// @author: Jonathan Jansson
/// </summary>

public class EditorController : MonoBehaviour{

    public static event PointCloudToggleDelegate OnPointCloudToggle;
    public delegate void PointCloudToggleDelegate();

    public LidarMenu lidarMenu;
    public GameObject mainCamera;
    public GameObject lidarCamera;
    public GameObject editorMenu;
    public GameObject pointCloud;
    public GameObject MainMenuButton;
    public Toggle visToggle;
    public Toggle pauseToggle;
    public Toggle lidarSensorToggle;
    public GameObject sceneEditor;

    /// <summary>
    /// Adds SetMode as a delegate "listener" to the play/stop button
    /// </summary>
    void Awake()
    {
        PlayButton.OnPlayToggled += SetMode;
    }
    void OnDestroy()
    {
        PlayButton.OnPlayToggled -= SetMode;
    }

    /// <summary>
    /// Sets activity of all necessary objects depending on if smulator or editor mode should be "on"
    /// </summary>
    /// <param name="simulatorMode"></param>
    void SetMode(bool simulatorMode)
    {
        if (!simulatorMode)
        {
            pauseToggle.isOn = false;
        }
        
        SwitchMainCameraView(simulatorMode);
        pauseToggle.interactable = simulatorMode;
        editorMenu.SetActive(!simulatorMode);
        lidarCamera.SetActive(lidarSensorToggle.isOn && !simulatorMode);
        SetVisToggleEnabled(simulatorMode);
        MainMenuButton.SetActive(!simulatorMode);
        sceneEditor.SetActive(!simulatorMode);
    }

    /// <summary>
    /// Sets the veiw and control settings of the main camera
    /// </summary>
    /// <param name="follow"></param>
	void SwitchMainCameraView(bool follow)
    {
		if(follow)
        {
			mainCamera.GetComponent<CameraController> ().SetFollow ();
		}
		else
        {
			mainCamera.GetComponent<CameraController> ().SetRoam ();
		}
	}

    /// <summary>
    /// Toggles the state of the point cloud toggle object and sets the activity of the point cloud object accordingly
    /// </summary>
    /// <param name="state"></param>
	void SetVisToggleEnabled(bool state)
    {
        visToggle.interactable = state;
        bool pointCloudActive = pointCloud.activeInHierarchy;

        if ((state && visToggle.isOn && !pointCloudActive) || (!state && pointCloudActive))
        {
            TogglePointCloudActive();
        }
	}

    /// <summary>
    /// Toggles the split screen visualisation through handling the activity of the point cloud and sets the main camera size to split or non split screen size
    /// </summary>
    public void TogglePointCloudActive()
    {
        pointCloud.SetActive(!pointCloud.activeInHierarchy);    

		if (!pointCloud.activeInHierarchy)
        {
           	mainCamera.GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
        }
        else
        {
            mainCamera.GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1);
            OnPointCloudToggle();
        }

        mainCamera.GetComponent<Camera>().enabled = true;
    }
}
