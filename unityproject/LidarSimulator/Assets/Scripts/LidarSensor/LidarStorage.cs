using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The data structure in which to save the lidar data.
///  @author: Tobias Alldén
/// </summary>
public class LidarStorage : MonoBehaviour {

    public delegate void Filled();
    public static event Filled HaveData;


	private Dictionary<float, List<LinkedList<SphericalCoordinate>>> dataStorage;

	public LidarStorage()
	{
		this.dataStorage = new Dictionary<float, List<LinkedList<SphericalCoordinate>>>();
        LidarSensor.OnScanned += Save;
	}

    void OnDestroy()
    {
        LidarSensor.OnScanned -= Save;
    }
    

    /// <summary>
    /// Saves the current collected points on the given timestamp. 
    /// </summary>
    /// <param name="newTime"></param>
    public void Save(float time, LinkedList<SphericalCoordinate> hits)
	{
        if (hits.Count != 0) {
            if (!dataStorage.ContainsKey(time))
            {
                List<LinkedList<SphericalCoordinate>> keyList = new List<LinkedList<SphericalCoordinate>>();
                keyList.Add(hits);
                dataStorage.Add(time, keyList);
            } else
            {
                dataStorage[time].Add(hits);
            }
        }		
	}


    public Dictionary<float, List<LinkedList<SphericalCoordinate>>> GetData()
    {
        return dataStorage;
    }

    public void SetData(Dictionary<float,List<LinkedList<SphericalCoordinate>>> data )
    {
        this.dataStorage = data;
        if(HaveData != null && data != null)
        {
            HaveData();
        }
    }
 


}
