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

/// <summary>
/// Manages the communication between the different scripts, or something, #yolo
/// </summary>
public class ExportManager : MonoBehaviour {

    public GameObject lidarStorageGameObject;
    public static event LoadingPoints Loading;
    public delegate void LoadingPoints(Coroutine load);


    private LidarStorage lidarStorage;

	// Use this for initialization
	void Start () {
        lidarStorageGameObject = GameObject.FindGameObjectWithTag("Lidar");
        lidarStorage = lidarStorageGameObject.GetComponent<LidarStorage>();		

	}


    /// <summary>
    /// Saves to the given filepath
    /// </summary>
    /// <param name="filePath"></param>
    public void Save(string filePath)
    {
        Dictionary<float, List<LinkedList<SphericalCoordinate>>> data = lidarStorage.GetData();

        if(data.Equals(null) || data.Count == 0)
        {
            Debug.Log("No data to save!");
        } else
        {
            SaveManager.SaveToCsv(data,filePath);
        }
    }

    /// <summary>
    /// Opens the given file
    /// </summary>
    /// <param name="filePath"></param>
    public void Open(string filePath)
    {
        
        Coroutine async = StartCoroutine(LoadManager.LoadCsv(filePath, lidarStorage));
        if (Loading != null)
        {
            Loading(async);
        }
        
    }

}



