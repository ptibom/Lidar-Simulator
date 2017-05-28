using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using System.Collections;

/// <summary>
/// The data structure in which to save the lidar data.
///  @author: Tobias Alldén
/// </summary>
public class LidarStorage : MonoBehaviour {

    public delegate void Filled();
    public static event Filled HaveData;


	private Dictionary<float, LinkedList<SphericalCoordinate>> dataStorage;
	private LinkedList<SphericalCoordinate> currentHits;
	private float prevTime; // Timestamp for previous data entry.

	public LidarStorage()
	{
		this.dataStorage = new Dictionary<float, LinkedList<SphericalCoordinate>>();
		this.currentHits = new LinkedList<SphericalCoordinate>();
        LidarSensor.OnScanned += AddHits;
        LidarSensor.NewRotationEvent += Save;
	}

    void OnDestroy()
    {
        LidarSensor.OnScanned -= AddHits;
        LidarSensor.NewRotationEvent -= Save;
    }

	/// <summary>
	/// Adds a single coorninate to the current hits LinkedList. 
	/// </summary>
	public void AddHit(SphericalCoordinate hit)
	{
		currentHits.AddLast(hit);
	}

    /// <summary>
    /// Adds coorninates to the currently collected hits. 
    /// </summary>
    public void AddHits(LinkedList<SphericalCoordinate> hits)
    {
        for (LinkedListNode<SphericalCoordinate> it = hits.First; it != null; it = it.Next)
        {
            currentHits.AddLast(it.Value);
        }
    }

    /// <summary>
    /// Saves the current collected points on the given timestamp. 
    /// </summary>
    /// <param name="newTime"></param>
    public void Save()
	{


        // Update the data structure if there is collected points. 
		if (currentHits.Count != 0)
		{
			dataStorage[prevTime] = currentHits;
			prevTime = Time.fixedTime;
            currentHits = new LinkedList<SphericalCoordinate>();
		}
	}


	/// <summary>
	/// Returns the last set of coordinates gathered. 
	/// </summary>
	/// <returns></returns>
	public LinkedList<SphericalCoordinate> GetLatestHits()
	{
		return currentHits;
	}

    public Dictionary<float, LinkedList<SphericalCoordinate>> GetData()
    {
        return dataStorage;
    }

    public void SetData(Dictionary<float,LinkedList<SphericalCoordinate>> data )
    {
        this.dataStorage = data;
        if(HaveData != null && data != null)
        {
            Debug.Log("Sending message: Length: " + data.Count);
            HaveData();
        }
    }
 


}
