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
        Dictionary<float, LinkedList<SphericalCoordinate>> data = lidarStorage.GetData();

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



