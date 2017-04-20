﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

/// <summary>
/// The data structure in which to save the lidar data.
///  @author: Tobias Alldén
/// </summary>
public class LidarStorage {

	private Dictionary<float, LinkedList<SphericalCoordinate>> dataStorage;
	private LinkedList<SphericalCoordinate> currentHits;
	private float prevTime; // Timestamp for previous data entry.


	public LidarStorage()
	{
		this.dataStorage = new Dictionary<float, LinkedList<SphericalCoordinate>>();
		this.currentHits = new LinkedList<SphericalCoordinate>();
        LidarSensor.OnScanned += AddHits;
        LidarSensor.StoreEvent += Save;
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
        foreach(SphericalCoordinate hit in hits)
        {
            currentHits.AddLast(hit);
        }
    }

    /// <summary>
    /// Saves the current collected points on the given timestamp. 
    /// </summary>
    /// <param name="newTime"></param>
    public void Save(float newTime)
	{
        // Update the data structure if there is collected points. 
		if (currentHits.Count != 0)
		{
			dataStorage[newTime] = currentHits;
			prevTime = newTime;
            currentHits.Clear();
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

}
