using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

/// <summary>
/// The data structure in which to save the lidar data.
///  @author: Tobias Alldén
/// </summary>
public class LidarStorage {

	private Dictionary<float, LinkedList<SphericalCoordinates>> data;
	private LinkedList<SphericalCoordinates> currentHits;
    private LinkedList<SphericalCoordinates> prevList;
	private float prevTime; // Timestamp for previous data entry.


	public LidarStorage()
	{
		this.data = new Dictionary<float, LinkedList<SphericalCoordinates>>();
		this.currentHits = new LinkedList<SphericalCoordinates>();
        LidarSensor.OnScanned += AddHits;
	}

	/// <summary>
	/// Adds a single coorninate to the current hits LinkedList. 
	/// </summary>
	public void AddHit(SphericalCoordinates hit)
	{
		currentHits.AddLast(hit);
	}

    /// <summary>
    /// Adds a coorninate to the current hits LinkedList. 
    /// </summary>
    public void AddHits(LinkedList<SphericalCoordinates> hits)
    {
        currentHits = hits;
    }

    /// <summary>
    /// Updates the points of the data structure
    /// </summary>
    /// <param name="newTime"></param>
    public void UpdatePoints(float newTime)
	{
		if (prevList == null && currentHits.Count != 0)
		{
			data[newTime] = currentHits;
			prevTime = newTime;
            prevList = currentHits;
			currentHits = new LinkedList<SphericalCoordinates>();
		}
		else
		{
            //new Thread(delegate ()
            //{
            //    Merge(newTime, currentHits, prevList, data);
            //}).Start();
            data[newTime] = currentHits;
            prevList = currentHits;
            currentHits = new LinkedList<SphericalCoordinates>();
            prevTime = newTime;
		}
	}

    /// <summary>
    /// Function for merging two LinkedLists and insert them into a data structure.
    /// </summary>
    /// <param name="newTime"></param>
    /// <param name="newCoordinates"></param>
    /// <param name="previousCoordinates"></param>
    /// <param name="dataStructure"></param>
    public static void Merge(float newTime, LinkedList<SphericalCoordinates> newCoordinates, LinkedList<SphericalCoordinates> previousCoordinates, Dictionary<float,LinkedList<SphericalCoordinates>> dataStructure) 
    {
        LinkedList<SphericalCoordinates> diffList = new LinkedList<SphericalCoordinates>();
        foreach(SphericalCoordinates sc in newCoordinates)
        {
            if(!previousCoordinates.Contains(sc))
            {
                diffList.AddLast(sc);
            }
        }
        LinkedList<SphericalCoordinates> newList = new LinkedList<SphericalCoordinates>();
        foreach(SphericalCoordinates sc in previousCoordinates)
        {
            if(newCoordinates.Contains(sc))
            {
                newList.AddLast(sc);
            }
        }
        foreach(SphericalCoordinates sc in diffList)
        {
            newList.AddLast(sc);
        }

        dataStructure[newTime] = newList;
    }

	/// <summary>
	/// Returns the last set of coordinates gathered. 
	/// </summary>
	/// <returns></returns>
	public LinkedList<SphericalCoordinates> GetLatestHits()
	{
		return currentHits;
	}


}
